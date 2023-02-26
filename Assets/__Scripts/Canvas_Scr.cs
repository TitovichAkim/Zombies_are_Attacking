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

    private Player_Scr      _playerScr;
    private PlayerHealth    _playerHealth;

    public void Start ()
    {
        _playerScr = player.GetComponent<Player_Scr>();
        _playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void Update ()
    {
        if(player != null && _playerHealth.hp > 0)
        {
            hp = _playerHealth.hp;
            loadMag = _playerScr.loadMagazine[_playerScr.activeWeapon];
            storeMag = _playerScr.storeMagazine;

            HP_Text.GetComponent<Text>().text = hp.ToString();
            HP_Dial.GetComponent<Image>().fillAmount = hp * 0.01f;

            loadMag_Text.GetComponent<Text>().text = loadMag + " / " + storeMag;
            loadMag_Dial.GetComponent<Image>().fillAmount = loadMag / storeMag;

            coinPanel.GetComponent<Text>().text = _playerScr.coin.ToString();        // Заполнить панель, отображающую колчество денег
        }
    }


}
