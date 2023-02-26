using UnityEngine;
[CreateAssetMenu(fileName = "EnemyesData", menuName = "Items/Enemyes")]
public class EnemyScriptableObject: ScriptableObject
{
    public string       nameEnemy;          // Наименование зомби
    public float        HP;                 // Уровень здоровья
    public float        speedOfMovement;    // Скорость движения
    public float        speedRotation;      // Скорость поворота
    public int          damage;             // Наносимый урон
    public float        reload;             // Время перезарядки серии ударов
    public int          numSerial;          // Количество ударов в серии
    public float        serialReload;       // Время перезарядки внутри серии
    public int          score;              // Количество очков, которое присваивается игроку за уничтожение
    public int          coin;               // Количество Coin, которое присваивается игроку за уничтожение
    public float        probalityBonus;     // Вероятность выпадения бонуса при уничтожении
    public float        attackingZoneRadius;// Радиус доступной противнику зоны атаки

    public GameObject   enemyPrefab;        // Префаб проивника
}
