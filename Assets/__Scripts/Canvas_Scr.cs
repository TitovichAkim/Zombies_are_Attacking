using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Scr:MonoBehaviour
{

    public float        hp;
    public int          score;

    public float        loadMag;
    public float        storeMag;


    public GameObject HP_BackGround;
    public GameObject HP_Dial;
    public GameObject HP_Text;
    public GameObject loadMag_Dial;
    public GameObject loadMag_Text;
    public GameObject coinPanel;
    public GameObject scoreBoard;
    public GameObject player;

    public Player_Scr playerScr;


    public void Start ()
    {
        playerScr = player.GetComponent<Player_Scr>();
    }

    public void Update ()
    {
        if(player != null && playerScr.hp > 0)
        {
            hp = playerScr.hp;
            loadMag = playerScr.loadMagazine[playerScr.activeWeapon];
            storeMag = playerScr.storeMagazine;

            HP_Text.GetComponent<Text>().text = hp.ToString();
            HP_Dial.GetComponent<Image>().fillAmount = hp * 0.01f;

            loadMag_Text.GetComponent<Text>().text = loadMag + " / " + storeMag;
            loadMag_Dial.GetComponent<Image>().fillAmount = loadMag / storeMag;

            coinPanel.GetComponent<Text>().text = playerScr.coin.ToString();        // Заполнить панель, отображающую колчество денег
        }
    }


}
