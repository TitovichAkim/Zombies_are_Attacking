using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript:MonoBehaviour
{

    public GameObject           MenuPanel;              // Содержит ссылку на панель Меню
    public GameObject           load_Panel;             // Содержит ссылку на панель загрузки
    public GameObject           communication_Panel;    // Содержит ссылку на панель ссылок
    public GameObject           highScoreText;          // Содержит ссылку на текст с лучшим результатом
    public GameObject           addBannerAnchor;        // Ссылка на рекламный якорь

    public GameObject[]         loadSlots;              // Содержит ссылки на кнопки загрузки игры

    public static string[]      SaveS = new string[5];  // Хранит имена слотов загрузок и сохранений
    public static int           openGameIndex = 0;      // Индекс вхождения в игру. После первого открытия главного меню становится 1 и больше не меняется

    public void Start ()
    {

        // Для того, чтобы сохранить дефолтные значения для неожиданного запуска новой игры 
        if(openGameIndex == 0)
        {
            // GameObject.Find("MenuANCHOR").GetComponent<SaveSerial>().SaveGame(100);
            openGameIndex++;
        }

        // Вот это нагромождение для того, чтобы у кнопок загрузки и сохранения с ходу были правильные названия
        for(int i = 0; i < SaveS.Length; i++)
        {
            if(PlayerPrefs.GetString("Save" + i).Length > 2)
            {
                SaveS[i] = PlayerPrefs.GetString("Save" + i);
            }
            if(SaveS[i] != null)
            {
                loadSlots[i].GetComponent<Text>().text = SaveS[i];
            }
        }
        highScoreText.GetComponent<Text>().text = "Лучший результат: " + Score_Board_Stat_Scr.highScore.ToString();     // Сделать запись о лучшем результате в игре
    }

    public void New_Game_Start ()
    {
        addBannerAnchor.GetComponent<YandexBunner>().banner.Destroy();
        Destroy(addBannerAnchor);
        SceneManager.LoadScene("1_Tower_Assault_0");
    }
    public void Load_Game_Start ()
    {
        MenuPanel.SetActive(false);
        load_Panel.SetActive(true);
        // StaticDATA.loadParameter = 1;
        // SceneManager.LoadScene("_sea");
    }

    public void GoExit ()
    {
        Application.Quit();
    }

    public void Close_Load_Panel ()
    {
        MenuPanel.SetActive(true);
        load_Panel.SetActive(false);
    }


    public void Open_Community_Panel ()
    {
        MenuPanel.SetActive(false);
        communication_Panel.SetActive(true);
    }

    public void Open_VK_Page ()
    {
        Application.OpenURL("https://vk.com/takda_creative_studio");
    }

    public void OpenDonationAlerts ()
    {
        Application.OpenURL("https://www.donationalerts.com/r/takda_cs");
    }
    public void Close_Community_Panel ()
    {
        MenuPanel.SetActive(true);
        communication_Panel.SetActive(false);
    }

}