using UnityEngine;

public class SurvivalGameManager : MonoBehaviour
{
    public Game_Menu_Scr    gameMenu;               // ��, ��� �� ����� ������ � ����, ���� ����� ����������� ������ ����� ������
    public bool             adBool = false;         // ����������, ������������� �� ��� ������� ��� �����������

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
