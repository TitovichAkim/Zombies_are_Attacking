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

    public Sprite[]         weaponSprites = new Sprite[10];                 // ������ �� ������ ������
    public Text             medicinesBaseText;                              // ������ �� ��������� ���� ����������� ����������
    public GameObject       shopCanvas;                                     // ������ �� ���� �������� � �������
    public GameObject       playerGO;                                       // ������ �� ������
    public GameObject       shopButton;                                     // ������ �� ������ �������� / �������� ��������

    public GameObject[]     weaponShopButtons;                              // ������ �� ����� � ������� � ��������
    public GameObject[]     ammunationShopButtons;                          // ������ �� ����� � ������������ � ��������
    public GameObject[]     equipmentShopButtons;                           // ������ �� ����� � �����������
    public GameObject[]     medicinesShopButtons;                           // ������ �� ����� � ������������ ����������� � ��������

    public WeaponsScriptableObject[] weaponsSOArray;                        // ������ �� ������
    public MedicinesScriptableObject[] medicinesSOArray;                    // ������ �� ������������

    private bool            openShopPanel = false;                          // ��������� �������� ���� �������� � �������
    public void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            shopButton.SetActive(true); // �������� ������ ��������
            RELOAD_ICONS();             // �������� ������ ������
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
    }       // ��� ������� ������ �������� / �������� ��������

    public void RELOAD_ICONS ()
    {
        int playersCoins = playerGO.GetComponent<Player_Scr>().coin;            // ������� ���������� � ����������� ����� � ������
        for(int i = 0; i < weaponsSOArray.Length; i++)
        {
            weaponShopButtons[i].GetComponent<Button>().interactable = weaponsSOArray[i].cost <= playersCoins;
        }
        for(int i = 0; i < medicinesSOArray.Length; i++)
        {
            medicinesShopButtons[i].GetComponent<Button>().interactable = medicinesSOArray[i].cost <= playersCoins;
        }

    } // ��������� ����� ������ ������

    #region ������� ���������
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