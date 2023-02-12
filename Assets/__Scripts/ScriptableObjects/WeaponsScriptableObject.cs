using UnityEngine;

[CreateAssetMenu(fileName = "MedicinesData", menuName = "Items/Weapons")]
public class WeaponsScriptableObject: ScriptableObject, ICostItem
{
    public Sprite          icon;                    // Иконка оружия
    public AudioClip       shotAudio;               // Звук выстрела
    public AudioClip       reloadWeapAudio;         // Звук перезарядки оружия

    public int             storeMagazine;           // 0 Ёмкость магазина
    public float           reloadMagazine;          // 1 Скорость перезарядки магазина
    public float           reloadTime;              // 2 Время между выстрелами
    public int             weaponDamage;            // 3 Наносимый урон
    public int             numberOfBullets;         // 4 Количество пуль, которые производит выстрел
    public int             bulletSpeed;             // 5 Скорость пули
    public int             cost;                    // 9 Стоимость оружия


    int ICostItem.cost                              // 9 Стоимость оружия
    {
        get
        {
            return (cost);
        }
        set
        {
            cost = value;
        }
    }                  
}
