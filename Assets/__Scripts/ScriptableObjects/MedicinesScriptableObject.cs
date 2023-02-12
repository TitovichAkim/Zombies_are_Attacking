using UnityEngine;
[CreateAssetMenu(fileName = "MedicinesData", menuName = "Items/Medicines")]

public class MedicinesScriptableObject: ScriptableObject, IItem
{
    public Sprite       icon;               // Иконка 
    public byte         timeOfAction;       // Время действия
    public float        rechargeTime;       // Время между применениями
    public int          cost;               // Стоимость
    public int          recoverableHealth;  // Восстанавливаемое здоровье
    public int          amount;             // Количество

    int IItem.amount
    {
        get; set;
    }
}
