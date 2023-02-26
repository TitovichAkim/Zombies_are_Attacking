using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface IItem
{
    int amount
    {
        get; set;
    }
}

public class PlayerInventory:MonoBehaviour
{
    public Player_Scr                       playerScript;   // ������ �� ������ Player_Scr
    public InventoryScrollUI                medicinesScroll;// ������ �� ������ ����������� ������                       
    public GameObject[]                     weaponBeltGO;   // ������, ���������� ������ ������ ������
    public List<MedicinesScriptableObject>  medcinesList;   // ������ ���������� � ��������� ������������
    public WeaponsScriptableObject[]        weaponBelt;     // ���� � �������. ���������� ����� ��������� ��������� ��������� � ��� �����
    public int                              lastModifiedIndex;  // ������ ���������� ����������� ��������

    private float                           _timeOfLastUse;
    private float                           _usageDelay;
    private int                             _hpPerSecond;
    private int                             _drugUsingTimer;
    private PlayerHealth                    _playerHealth;

    public void Start ()
    {
        _InitialisationLists();                             // ���������������� ������
        playerScript = GetComponent<Player_Scr>();          // ��������� ������ �� ������ Player_Scr
        _playerHealth = GetComponent<PlayerHealth>();       // ��������� ������ �� ��������������
    }
    private void _InitialisationLists ()
    {
        medcinesList = new List<MedicinesScriptableObject>();               // ������������� ������ ������������ � ���������
    }

    public void BuyWeapons (WeaponsScriptableObject weaponSO)               // ���������� ��� ������� �� ������ ������� ������
    {
        int activeBeltSlot = GetComponent<Player_Scr>().activeWeapon;       // �������� �������� ���� ������
        weaponBelt[activeBeltSlot] = weaponSO;                              // �� ��������� ���� � ��������������� ���� �������� ������
        weaponBeltGO[activeBeltSlot].GetComponent<Image>().sprite = weaponSO.icon;  // �������� ������ ������ � ������ ������
        playerScript.TakeAWeapon(activeBeltSlot);                           // ����� ������

    }
    public void AddItem (object newItem)                           // ���������� ��������� � ��������� (������� �� ��������� ��, ������������ ��� ������������� ���������)
    {
        switch(newItem)                                                     // � ����������� �� ��������� �� ���������� ���������, � ������� �������� �������
        {
            case MedicinesScriptableObject medicines:                       // ���� � ������� ����� ���. ��������
                if(!medcinesList.Contains(medicines))                       // ���� � ������ ��� ������� ��������
                {
                    medicinesScroll.CreateMedicinesButton(medicines);       // ������� � ����� � ��� ���������� ��������
                    CheckItem(medcinesList, medicines, 1);                  // �������� ������� (������ ���. ����������, ��, +1 - ��������)
                }
                else
                {
                    CheckItem(medcinesList, medicines, 1);                  // �������� ������� (������ ���. ����������, ��, +1 - ��������)
                    medicinesScroll.ModifiedIndexOnButton(+1);              // �������� ���������� ��������� � ��������� ���������
                }   // ��� �������� ��������� CheckItem(), ��� ��� ��� ������ ��������� ��� ������� ������ ���� ������� � ������ �����

                break;
        }
    }
    public void RemoveItem (object newItem)                        // �������������/�������� ��������� �� ��������� (������� �� ��������� ��, ������������ ��� ������������� ���������)
    {
        switch(newItem)                                                     // � ����������� �� ��������� �� ���������� ���������, � ������� �������� �������
        {
            case MedicinesScriptableObject medicines:                       // ���� � ������� ����� ���. ��������
                if (_usageDelay <= Time.time - _timeOfLastUse)              // ���� ������ ���������� ������� ����� ���������� ����������
                {
                    UseTheDrug(medicines);                                      // ������������ ��������
                    CheckItem(medcinesList, medicines, -1);                     // ������ ������� (������ ���. ����������, ��, -1 - ������)
                    if(!medcinesList.Contains(medicines))                       // ���� � ������ ��� ������� ��������
                    {
                        medicinesScroll.RemoveMedicinesButton(lastModifiedIndex);// ������� � ����� � ��� ���������� ��������
                    }
                    else
                    {
                        medicinesScroll.ModifiedIndexOnButton(-1);              // �������� ���������� ��������� � ��������� ���������
                    }
                }
                break;
        }
    }
    public void UseTheDrug (MedicinesScriptableObject drug)
    {
        if (drug.timeOfAction == 0)
        {
            _playerHealth.hp += drug.recoverableHealth;
            _timeOfLastUse = Time.time;
        }
        else
        {
            _hpPerSecond = drug.recoverableHealth;
            _drugUsingTimer = drug.timeOfAction;
            _UseTheDrug();
        }
        _usageDelay = drug.rechargeTime;
        _timeOfLastUse = Time.time;
    }

    private void _UseTheDrug ()
    {
        if (_drugUsingTimer > 0)
        {
            _drugUsingTimer -= 1;
            _playerHealth.hp += _hpPerSecond;
            Invoke("_UseTheDrug", 1f);
        }

    }

    private void CheckItem<T> (List<T> list, T item, int num)
    where T : IItem
    {

        if(list.Contains(item))
        {
            lastModifiedIndex = list.IndexOf(item);
            list[list.IndexOf(item)].amount += num;
            if (list[list.IndexOf(item)].amount <= 0)
            {
                list.Remove(item);
            }
        }
        else
        {
            list.Add(item);
            list[list.IndexOf(item)].amount += num;
            lastModifiedIndex = list.IndexOf(item);
        }
        // lastModifiedIndex = list.IndexOf(item) ������������ ������, ��� ��� ��������� ��� ��������� � ���������� ������ �������
    }

}
