using System.Collections.Generic;
using UnityEngine;
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
    [Header("Set in Inspector")]
    public float        HP;                 // Уровень здоровья
    public int          damage;             // Наносимый урон
    public float        serialReload;       // Время перезарядки серии ударов
    public int          numSerial;          // Количество ударов в серии
    public float        reload;             // Время перезарядки внутри серии
    public int          score;              // Количество очков, которое присваивается игроку за уничтожение
    public int          coin;               // Количество Coin, которое присваивается игроку за уничтожение
    public float        probalityBonus;     // Вероятность выпадения бонуса при уничтожении

    public List<AudioClip> randomSounds;    // Рандомные звуки зомби
    public List<AudioClip> biteSounds;      // Звуки укусов
    public List<AudioClip> hitSounds;       // Звуки попаданий пулями



    [Header("Set Dynamically")]
    public bool         attInd = false;         // Режим атаки
    public bool         forcedGoalBool = false; // Является ли этот объект приоритетным

    public float        attackMoment;           // Момент последнего удара
    public float        theMomentOfGrowling;    // Момент последнего рычания
    public float        timerOfGrowling;        // Таймер рычания

    public EnemyNavigation navigation;          // Ссылка на скрипт навигации
    public Enemy_Spawner_Scr spawnerScr;        // Ссылка на скрипт спаунера

    public GameObject   spawner;        // Ссылка на спаунер
    public GameObject   player;         // Ссылка на игрока
    public GameObject   targetGO;       // Целевой объект
    public Vector2      playerPos;      // Координаты позиции игрока
    public Vector2      goPos;          // Координаты позиции объекта

    public Rigidbody2D rigid2D;        // Ссылка на Rigidbody2d объекта
    public AudioSource audioSource;    // Ссылка на компонент, воспроизводящий звуки

    // Параметры, которые зависят от типа объекта
    public float        moveSpeed;     // Скорость движения



    public void Start ()
    {
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();      // Заполнить ссылку на Rigidbody2d объекта
        navigation = this.gameObject.GetComponent<EnemyNavigation>(); // Заполнить ссылку на скрипт навигации

        timerOfGrowling = Random.Range(4f, 20f);                    // При создании объекта записать новый таймер рычания
        theMomentOfGrowling = Time.time;                            // и текущий момент, чтобы сразу никто не рычал
        CREATE_AUDIO_SOURSE();
    }

    public void FixedUpdate ()
    {
        playerPos = player.transform.position;
        goPos = this.gameObject.transform.position;

        if (state == enemyState.attacking)
        {
            if(targetGO != null)
            {
                ATTACK(targetGO);       // Атаковать
            }
            else
            {
                StateRedactor(player);
            }
        }


        if(timerOfGrowling <= Time.time - theMomentOfGrowling)
        {   // Если таймер рычания меньше, чем разница между текущим моментом и моментом последнего рычания
            PLAY_AUDIO(0);                                          // Воспроизвести звук рычания
            theMomentOfGrowling = Time.time;                        // Записать момент последнего рычания
            timerOfGrowling = Random.Range(4f, 20f);                // Создать новый таймер до следующего рычания
        }

    }

    public void OnTriggerEnter2D (Collider2D collision)         // Если вошел в соприкосновение с ключевым объектом (пуля, игрок, стена)
    {

        if(collision.gameObject.layer == 8)                                                     // Если колиззионный объект - пуля
        {                                                                                       // Если коллайдер - bullet
            HP -= collision.gameObject.GetComponent<Bullet_Scr>().damage;                       // Отнять HP равное damage из пули
            PLAY_AUDIO(1);                                                                      // Воспроизвести звук попадания в туловище пули
            if(HP <= 0)
            {                                                                      // Если HP меньше или равно нуля
                Destroy(this.gameObject);                                                       // Уничтожить объект
                spawnerScr.enemyList.Remove(this.gameObject);    // Удалить объект из списка врагов, находящихся на сцене
                spawnerScr.score += score;                       // Добавить очки при убийстве врага
                player.GetComponent<Player_Scr>().coin += coin;                                 // Добавить Coin при убийстве врага
                spawnerScr.SCORE_BOARD_RELOAD();                 // Перезагрузить доску очков
                if(spawnerScr.enemyList.Count == 0)              // Если список врагов на сцене опустел
                {
                    spawnerScr.startTime = Time.time;            // Записать момент, когда закончилась волна
                }
            }
        }
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 13)                // Если коснулся игрока или двери
        {
            StateRedactor(collision.gameObject);                                                // Вызвать редактор состояния
        }

    }
    /*
    public void OnTriggerStay2D (Collider2D collision)          // Если вошел в соприкосновение с дверью
    {
        if(collision.gameObject.layer == 13 && reload <= Time.time - attackMoment && collision.gameObject.GetComponent<Door_Scr>().doorHp > 0)
        {
            navigation.SetTarget(collision.transform);
            attackMoment = Time.time;
            collision.gameObject.GetComponent<Door_Scr>().doorHp -= damage;
            collision.gameObject.GetComponent<Door_Scr>().PLAY_AUDIO(2);
        }
    }*/


    public void ATTACK (GameObject target)
    {
        attackMoment = Time.time;
        if (target.layer == 6)
        {
            target.GetComponent<Player_Scr>().hp -= damage;
            PLAY_AUDIO(1);  // Воспроизвести звук укуса
        }
        else if (target.layer == 13)
        {
            target.GetComponent<Door_Scr>().doorHp -= damage;
            target.GetComponent<Door_Scr>().PLAY_AUDIO(2);
        }

        /*
        if(Vector3.Distance(playerPos, goPos) <= 1.1f & reload <= Time.time - attackMoment)
        {
            attackMoment = Time.time;
            player.GetComponent<Player_Scr>().hp -= damage;

            PLAY_AUDIO(1);  // Воспроизвести звук укуса
        }*/
    }

    public void CREATE_AUDIO_SOURSE ()
    {
        this.gameObject.AddComponent<AudioSource>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 1;
    } // Создает компонент, воспроизводящий звуки зомби и настраивает его

    public void PLAY_AUDIO (int aud)
    {
        if(audioSource.clip == null)
        {
            switch(aud)
            {
                case 0:     // Рандомный рык или что-то еще 
                    audioSource.PlayOneShot(randomSounds[Random.Range(0, randomSounds.Count - 1)]);
                    break;
                case 1:     // Звуки укусов
                    audioSource.PlayOneShot(biteSounds[Random.Range(0, biteSounds.Count - 1)]);
                    break;
                case 2:     // Звуки попадания в тело патронов
                    audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Count - 1)]);
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

    public void StateRedactor (GameObject target = null)
    {

        if (target == null)
        {
            state = enemyState.stay;
        }

        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, this.gameObject.transform.position) > 1.5f) // Если дистанция до цели не в пределах 1.5 метров
            {
                state = enemyState.stalking;                                                            // Включить режим движения к цели
                navigation.agent.speed = moveSpeed;                                                     // Возобновить скорость навигации
            }
            else
            {
                                                                                                        // Если дистанция до цели не в пределах 1.5 метров
            }
            {
                state = enemyState.attacking;                                                           // Включить режим атаки
                navigation.agent.speed = 0;                                                             // Скорость - ноль
            }
            targetGO = target;                                                                          // Целевой игровой объект - расширяющая цель
        }
        
    }
}
