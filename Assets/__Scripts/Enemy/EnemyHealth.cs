using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageabel
{
    public Player_Scr               playerScript;       // Ссылка на скрипт игрока
    public Enemy_Spawner_Scr        spawner;            // Ссылка на скрипт спаунера
    public EnemyScriptableObject    enemyParametersSO;  // Ссылка на СО противника

    private float       _hp;                            // Уровень здоровья
    private Enemy_Scr   _enemyScript;                   // Ссылка на основной скрипт

    public float hp
    {
        get
        {
            return (_hp);
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                DestroyEnemy();
                _enemyScript.StateRedactor(null, true);
            }
        }
    }

    private void Awake ()
    {
        _enemyScript = GetComponent<Enemy_Scr>();
    }
    private void Start ()
    {
        hp = enemyParametersSO.HP;      // Инициализировать значение здоровья
    }
    public void ApplyDamage (int damageValue)
    {
        hp -= damageValue;              // Получить урон
        _enemyScript.PLAY_AUDIO(1);     // Воспроизвести звук получения урона
    }

    private void DestroyEnemy ()
    {
        spawner.enemyList.Remove(gameObject);       // Убрать этого врага из списка врагов
        spawner.score += enemyParametersSO.score;   // Начислить очки за уничтожение
        playerScript.coin += enemyParametersSO.coin;// Начислить монет за уничтожение
    }
}
