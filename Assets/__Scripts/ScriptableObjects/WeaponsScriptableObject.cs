using UnityEngine;

[CreateAssetMenu(fileName = "MedicinesData", menuName = "Items/Weapons")]
public class WeaponsScriptableObject: ScriptableObject, ICostItem
{
    public Sprite          icon;                    // ������ ������
    public AudioClip       shotAudio;               // ���� ��������
    public AudioClip       reloadWeapAudio;         // ���� ����������� ������

    public int             storeMagazine;           // 0 ������� ��������
    public float           reloadMagazine;          // 1 �������� ����������� ��������
    public float           reloadTime;              // 2 ����� ����� ����������
    public int             weaponDamage;            // 3 ��������� ����
    public int             numberOfBullets;         // 4 ���������� ����, ������� ���������� �������
    public int             bulletSpeed;             // 5 �������� ����
    public int             cost;                    // 9 ��������� ������


    int ICostItem.cost                              // 9 ��������� ������
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
