using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface ICostItem
{
    int cost
    {
        get; set;
    }
}
public class Shop_Scr: MonoBehaviour
{

    public Sprite[]         weaponSprites = new Sprite[10];                 // Ссылка на иконки оружия
    public Text             medicinesBaseText;                              // Ссылка на текстовую базу медицинских препаратов
    public GameObject       shopCanvas;                                     // Ссылка на окно магазина с оружием
    public GameObject       playerGO;                                       // Ссылка на игрока
    public GameObject       shopButton;                                     // Ссылка на кнопку открытия / закрытия магазина

    public GameObject[]     weaponShopButtons;                              // Ссылка на слоты с оружием в магазине
    public GameObject[]     ammunationShopButtons;                          // Ссылка на слоты с боеприпасами в магазине
    public GameObject[]     equipmentShopButtons;                           // Ссылка на слоты с экипировкой
    public GameObject[]     medicinesShopButtons;                           // Ссылка на слоты с медицинскими препаратами в магазине

    public WeaponsScriptableObject[] weaponsSOArray;                        // Массив СО оружия
    public MedicinesScriptableObject[] medicinesSOArray;                    // Массив СО медикаментов

    private bool            openShopPanel = false;                          // Состояние открытия окна магазина с оружием
    public void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            shopButton.SetActive(true); // Включить панель магазина
            RELOAD_ICONS();             // Обновить иконки оружия
        }
    }

    public void OnTriggerExit2D (Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            shopButton.SetActive(false);
        }
    }

    public void Shop_Button_Click ()
    {
        if(openShopPanel == false)
        {
            shopCanvas.SetActive(true);
            openShopPanel = true;
            RELOAD_ICONS();
            Time.timeScale = 0;
        }
        else
        {
            shopCanvas.SetActive(false);
            openShopPanel = false;
            Time.timeScale = 1;
        }
    }       // При нажатии кнопки открытия / закрытия магазина

    public void RELOAD_ICONS ()
    {
        int playersCoins = playerGO.GetComponent<Player_Scr>().coin;            // Создать переменную с количеством денег у игрока
        for(int i = 0; i < weaponsSOArray.Length; i++)
        {
            weaponShopButtons[i].GetComponent<Button>().interactable = weaponsSOArray[i].cost <= playersCoins;
        }
        for(int i = 0; i < medicinesSOArray.Length; i++)
        {
            medicinesShopButtons[i].GetComponent<Button>().interactable = medicinesSOArray[i].cost <= playersCoins;
        }

    } // Обновляет цвета иконок оружия

    #region ПОКУПКА ПРЕДМЕТОВ
    public void BuyWeapon (WeaponsScriptableObject weaponSO)
    {
        playerGO.GetComponent<PlayerInventory>().BuyWeapons(weaponSO);
        playerGO.GetComponent<Player_Scr>().coin -= weaponSO.cost;
        RELOAD_ICONS();
    }

    public void BuyMedicines (MedicinesScriptableObject medicinesGO)
    {
        playerGO.GetComponent<PlayerInventory>().AddItem(medicinesGO);
        playerGO.GetComponent<Player_Scr>().coin -= medicinesGO.cost;
        RELOAD_ICONS();
    }
    #endregion
}