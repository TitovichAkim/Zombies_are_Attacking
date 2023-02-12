using UnityEngine;
[CreateAssetMenu(fileName = "MedicinesData", menuName = "Items/Medicines")]

public class MedicinesScriptableObject: ScriptableObject, IItem
{
    public Sprite       icon;               // ������ 
    public byte         timeOfAction;       // ����� ��������
    public float        rechargeTime;       // ����� ����� ������������
    public int          cost;               // ���������
    public int          recoverableHealth;  // ����������������� ��������
    public int          amount;             // ����������

    int IItem.amount
    {
        get; set;
    }
}
