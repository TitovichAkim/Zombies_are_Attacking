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
    public Player_Scr                       playerScript;   // Ссылка на скрипт Player_Scr
    public InventoryScrollUI                medicinesScroll;// Ссылка на скрипт медицинской панели                       
    public GameObject[]                     weaponBeltGO;   // Массив, содержащий кнопки слотов оружия
    public List<MedicinesScriptableObject>  medcinesList;   // Список хранящихся в инвентаре медикаментов
    public WeaponsScriptableObject[]        weaponBelt;     // Пояс с оружием. Изначально через инспектор присвоены пистолеты в два слота
    public int                              lastModifiedIndex;  // Индекс последнего измененного предмета

    private float                           _timeOfLastUse;
    private float                           _usageDelay;
    private int                             _hpPerSecond;
    private int                             _drugUsingTimer;

    public void Start ()
    {
        _InitialisationLists();                             // Инициализировать списки
        playerScript = GetComponent<Player_Scr>();          // Заполнить ссылку на скрипт Player_Scr
    }
    private void _InitialisationLists ()
    {
        medcinesList = new List<MedicinesScriptableObject>();               // Инициализация списка медикаментов в инвентаре
    }

    public void BuyWeapons (WeaponsScriptableObject weaponSO)               // Вызывается при нажатии на кнопки покупки оружия
    {
        int activeBeltSlot = GetComponent<Player_Scr>().activeWeapon;       // Записать активный слот оружия
        weaponBelt[activeBeltSlot] = weaponSO;                              // На оружейный пояс в соответствующий слот повесить оружие
        weaponBeltGO[activeBeltSlot].GetComponent<Image>().sprite = weaponSO.icon;  // Изменить иконку оружия в слотах игрока
        playerScript.TakeAWeapon(activeBeltSlot);                           // Взять оружие

    }
    public void AddItem (object newItem)                           // Добавление предметов в инвентари (зависит от входящего СО, предназначен для накапливаемых предметов)
    {
        switch(newItem)                                                     // В зависимости от входящего СО выбирается инвентарь, в который добавить предмет
        {
            case MedicinesScriptableObject medicines:                       // Если в АддИтем зашел мед. препарат
                if(!medcinesList.Contains(medicines))                       // Если в списке нет данного предмета
                {
                    medicinesScroll.CreateMedicinesButton(medicines);       // Удалить к нопку с его предыдущим индексом
                    CheckItem(medcinesList, medicines, 1);                  // Добавить предмет (список мед. препаратов, СО, +1 - добавить)
                }
                else
                {
                    CheckItem(medcinesList, medicines, 1);                  // Добавить предмет (список мед. препаратов, СО, +1 - добавить)
                    medicinesScroll.ModifiedIndexOnButton(+1);              // Изменить количество предметов в текстовом документе
                }   // Тут пришлось повторить CheckItem(), так как при разных раскладах эта функция должна быть вызвана в разное время

                break;
        }
    }
    public void RemoveItem (object newItem)                        // Использование/удаление предметов из инвентаря (зависит от входящего СО, предназначен для накапливаемых предметов)
    {
        switch(newItem)                                                     // В зависимости от входящего СО выбирается инвентарь, в который добавить предмет
        {
            case MedicinesScriptableObject medicines:                       // Если в АддИтем зашел мед. препарат
                if (_usageDelay <= Time.time - _timeOfLastUse)              // Если прошло достаточно времени после последнего применения
                {
                    UseTheDrug(medicines);                                      // Использовать препарат
                    CheckItem(medcinesList, medicines, -1);                     // Убрать предмет (список мед. препаратов, СО, -1 - убрать)
                    if(!medcinesList.Contains(medicines))                       // Если в списке нет данного предмета
                    {
                        medicinesScroll.RemoveMedicinesButton(lastModifiedIndex);// Удалить к нопку с его предыдущим индексом
                    }
                    else
                    {
                        medicinesScroll.ModifiedIndexOnButton(-1);              // Изменить количество предметов в текстовом документе
                    }
                }
                break;
        }
    }
    public void UseTheDrug (MedicinesScriptableObject drug)
    {
        if (drug.timeOfAction == 0)
        {
            playerScript.hp += drug.recoverableHealth;
            _timeOfLastUse = Time.time;
        }
        else
        {
            _hpPerSecond = drug.recoverableHealth;
            _drugUsingTimer = drug.timeOfAction;
            _UseTheDrug();
        }
        if(playerScript.hp > 100)
        {
            playerScript.hp = 100;
        }
        _usageDelay = drug.rechargeTime;
        _timeOfLastUse = Time.time;
    }

    private void _UseTheDrug ()
    {
        if (_drugUsingTimer > 0)
        {
            _drugUsingTimer -= 1;
            playerScript.hp += _hpPerSecond;
            Invoke("_UseTheDrug", 1f);
            if(playerScript.hp > 100)
            {
                playerScript.hp = 100;
            }
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
        // lastModifiedIndex = list.IndexOf(item) присутствует дважды, так как требуется его изменение в правильный момент времени
    }

}
