using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum enemyState
{
    stay,       // стоит на месте
    walk,       // блуждает
    stalking,   // преследует
    attacking,  // атакует
    dead        // мертв
}
public class Enemy_Scr:MonoBehaviour
{
    public enemyState   state;

    [Header("SetInSpawner")]
    // GameObjects
    public GameObject               spawner;                // Ссылка на спаунер (назначается спаунером)
    public GameObject               player;                 // Ссылка на игрока (назначается спаунером)
    public GameObject               attackingZone;          // Ссылка на зону атаки (назначается спаунером)
    // Classes
    public EnemyNavigation          navigation;             // Ссылка на скрипт навигации (назначается спаунером)
    public Enemy_Spawner_Scr        spawnerScr;             // Ссылка на скрипт спаунера (назначается спаунером)
    // ScriptableObjects
    public EnemyScriptableObject    enemyParametersSO;      // Ссылка на СО противника (назначается спаунером)
    public SoundsScriptableObject   sounds;                 // Ссылка на звуковую базу (назначается спаунером)

    [Header("SetInStart")]

    public Rigidbody2D              rigid2D;                // Ссылка на Rigidbody2d объекта
    private EnemyAttacking          _enemyAttacking;        // Ссылка на скрипт атаки (назначается в старте)
    public AudioSource              audioSource;            // Ссылка на компонент, воспроизводящий звуки

    [Header("SetDynamically")]
    public bool                     forcedGoalBool = false; // Является ли этот объект приоритетным
    private GameObject              _targetGO;              // Целевой объект 

    public void Start ()
    {
        _enemyAttacking = GetComponent<EnemyAttacking>();               // Заполнить ссылку на скрипт атаки
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();          // Заполнить ссылку на Rigidbody2d объекта

        //attackingZone.GetComponent<CircleCollider2D>().radius = enemyParametersSO.attackingZoneRadius;
        attackingZone.GetComponent<EnemyAttackingZone>().enemyScript = this;
        CREATE_AUDIO_SOURSE();
    }

    public void CREATE_AUDIO_SOURSE ()      // Создает компонент, воспроизводящий звуки зомби и настраивает его
    {
        this.gameObject.AddComponent<AudioSource>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 1;
    }

    public void PLAY_AUDIO (int aud)
    {
        if(audioSource.clip == null)
        {
            switch(aud)
            {
                case 0:     // Рандомный рык или что-то еще 
                    audioSource.PlayOneShot(sounds.zombieWalkingSounds[Random.Range(0, sounds.zombieWalkingSounds.Length)]);
                    break;
                case 1:     // Звуки укусов
                    audioSource.PlayOneShot(sounds.zombieBiteSounds[Random.Range(0, sounds.zombieBiteSounds.Length)]);
                    break;
                case 2:     // Звуки попадания в тело патронов
                    audioSource.PlayOneShot(sounds.zombieHitSounds[Random.Range(0, sounds.zombieHitSounds.Length)]);
                    break;
            }
        }
    }
    public void SELECT_AN_OBJECT ()
    {                                                                            // Выполняет функцию выделения иля снятия приоритетной цели
        if(forcedGoalBool == false)
        {                                                                           // Если эта цель не приоритетная
            if(player.GetComponent<Player_Scr>().forcedGoal != null)
            {                                             // Если у игрока была другая приоритетная цель
                player.GetComponent<Player_Scr>().forcedGoal.GetComponent<Enemy_Scr>().forcedGoalBool = false;      // Снять приоритеть у старой приоритетной цели
                player.GetComponent<Player_Scr>().forcedGoal.gameObject.GetComponent<SpriteRenderer>().enabled = false; // Выключить спрайт "кругляшок" на предыдущем противнике
            }
            player.GetComponent<Player_Scr>().forcedGoal = this.gameObject;                                         // Записать эту цель у игрока как приоритетную
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;                                          // Включить спрайт "кругляшок"
            forcedGoalBool = true;                                                                                  // Сказать этой цели, что она приоритетная
        }
        else
        {                                                                                            // Если эта цель была приоритетной
            player.GetComponent<Player_Scr>().forcedGoal = null;                                                    // Очистить приоритет у игрока
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;                                         // Выключить спрайт "кругляшок"
            forcedGoalBool = false;                                                                                 // Сказать этой цели, что она больше не приоритетная
        }
    }   // При нажатии на этот объект, гооврит игроку повернуться в его сторону (выделить)

    public void OnMouseDown ()
    {
        SELECT_AN_OBJECT();         // Редактирует приоритеты цели
    }

    public void StateRedactor (GameObject target = null, bool destroy = false)
    {
        if(destroy)
        {
            state = enemyState.dead;
        } 
        else
        {
            if(target != null)
            {
                _targetGO = target;
                state = enemyState.attacking;                                                           // Включить режим атаки
            } 
            else
            {
                if(StaticGameManager.gameMode != "survival")
                {
                    state = enemyState.stay;
                }
                else
                {
                    state = enemyState.stalking;                                                            // Включить режим движения к цели
                }
            }
        }
        СhangeState();  // Выполнить все действия по назначенному состоянию
    }

    private void СhangeState () // Изменяет параметры противника в зависимости от его нового состояния
    {

        switch(state)
        {
            case enemyState.stay:

                break;
            case enemyState.walk:
                RepeatTheSound();
                break;
            case enemyState.stalking:

                navigation.agent.speed =  enemyParametersSO.speedOfMovement;                            // Возобновить скорость навигации
                navigation.InvokeRepeating("UpdatePlayerPosition", 0f, 1f);                             // Запустить цикл перерисовки пути к цели
                RepeatTheSound();
                break;

            case enemyState.attacking:

                navigation.agent.speed = 0;                                                             // Скорость - ноль
                _enemyAttacking.targetGO = _targetGO;                                                   // назначить цель для атаки
                StartCoroutine(_enemyAttacking.AttackTimer());                                          // Запустить корутину атаки
                break;

            case enemyState.dead:
                Destroy(gameObject);                                                                    // Уничтожить объект
                break;
        }
    }

    private void RepeatTheSound ()      // Повторяет указанный звук. Пока это звук при приследовании и прогулке
    {
        if (state == enemyState.walk || state == enemyState.walk)
        {
            PLAY_AUDIO(0);                                          // Воспроизвести звук рычания
            Invoke("RepeatTheSound", Random.Range(4f, 20f));        // Назначить повторение этого метода через рандомный промежуток времени
        }
    }
}
