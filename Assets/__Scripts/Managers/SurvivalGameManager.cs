using UnityEngine;

public class SurvivalGameManager : MonoBehaviour
{
    public Game_Menu_Scr    gameMenu;               // “о, что мы будем делать в меню, надо будет реализовать внутри этого класса
    public bool             adBool = false;         // ѕоказывает, запрашивалась ли уже реклама дл€ возрождени€

    public void OpenAdForHPAnswer()
    {
        if(adBool == false)
        {
            gameMenu.OPEN_AD_FORHP_ANSWER("YES");
        }
        else
        {
            gameMenu.OPEN_AD_FORHP_ANSWER("NO");
        }
    }
}
