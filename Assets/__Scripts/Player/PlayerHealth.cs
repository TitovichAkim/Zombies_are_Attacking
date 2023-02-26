using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageabel
{

    public bool             adBool = false;         // ѕоказывает, запрашивалась ли уже реклама дл€ возрождени€

    private int             _hp = 100;
    private Player_Scr      _playerScript;
    private Game_Menu_Scr   _gamewMenu;


    private void Awake ()
    {
        _playerScript = GetComponent<Player_Scr>();
        _gamewMenu = _playerScript.gameMenu.GetComponent<Game_Menu_Scr>();      // ѕридетс€ пока использовать костыль. 
    }

    public int hp
    {
        get
        {
            return (_hp);
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                if(adBool == false)
                {
                    _gamewMenu.OPEN_AD_FORHP_ANSWER("YES");
                }
                else
                {
                    _gamewMenu.OPEN_AD_FORHP_ANSWER("NO");
                }
            }
            if (_hp >= 100)
            {
                _hp = 100;
            } 
        }
    }
    public void ApplyDamage (int damageValue)
    {
        hp -= damageValue;
    }
}
