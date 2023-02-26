using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Menu_Scr:MonoBehaviour
{
    public GameObject           gameMenuGO;         // ������ �� ������ �������� ����
    public GameObject           deathText;          // ������ �� ����� � ������
    public GameObject           scoreText;          // ������ �� ����� "���� ����"
    public GameObject           scoreBoardText;     // ������ �� ������������� ����� � �������
    public GameObject           congratilateText;   // ������ �� ������������, ������� ����������, ���� ���� ������, ��� ��������� ������
    public GameObject           mainMenuButton;     // ������ �� ������ ������ � ������� ���� ����� ������
    public GameObject           mainCamera;         // ������ �� ������� ������

    public GameObject           playerGO;

    public Canvas               deathCanvas;        // ����� ������������ ���������� ��������
    public GameObject           gameOverPanel;      // ������ �� ����� ���������� ����������� ������
    public GameObject           adHealthPanel;      // ���� ������� ��� ��������� ������� ��� �����������
    public bool                 adForHPAnswerBool = false; // ����������� �� ��� ��� ����

    public GameObject           InputCanvas;        // ������ �� ������, ������� ����� ������ ��� �������� ����



    public void OPEN_MENU ()
    {           // ���������� ������� �������� ���� � ������ ������� ����
        gameMenuGO.SetActive(true);     // �������� ������� ����
        Time.timeScale = 0;             // ���������� ����� (��������� ���� �� �����)
    }
    public void CLOSE_MENU ()
    {          // ������ "����������"
        gameMenuGO.SetActive(false);    // ������ ������� ����
        Time.timeScale = 1;             // ����������� ���� (��������� ��������������� �������)
    }
    public void NEW_GAME_BUTTON ()
    {     // ���������� ������� "����� ����"
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // ��������� ��� ����� � ������
    }
    public void MAIN_MENU_BUTTON ()
    {                // ���������� ������� "������� ����"
        SceneManager.LoadScene("0_Main_Menu");      // ��������� ����� �������� ����
    }
    public void EXIT_GAME ()
    {           // ���������� ������� "����� �� ����"
        Application.Quit();             // ��� �������� ������� ������ �� ����
    }

    public void PLAYER_DESTROY (int score)           // ���������� �� ������� Player ��� �������� ������
    {
        InputCanvas.SetActive(false);                // ������ ����������� 
        deathCanvas.gameObject.SetActive(true);     // �������� ����� ���������� ����

        scoreBoardText.GetComponent<Text>().text = score.ToString();

        if(score > Score_Board_Stat_Scr.highScore)
        {                       // ���� ���������� ����� � ���� ���� ������, ��� ������ ���������
            Score_Board_Stat_Scr.highScore = score;                         // �������� ����� ������ ���������
            congratilateText.SetActive(true);                               // ������������ ��������������� ������� 
            scoreBoardText.GetComponent<Text>().color = Color.yellow;       // ��������� ������� ����� � ������ (���� ������)
        }
        Time.timeScale = 0;             // ���������� ����� (��������� ���� �� �����)
    }

    public void PLAYER_WIN (int score)
    {             // ���������� �� ������� Enemy_Spawner_Scr, ����� ����������� ��������� �����
        InputCanvas.SetActive(false);               // ������ ����������� 
        deathCanvas.gameObject.SetActive(true);     // �������� ����� ���������� ����

        deathText.GetComponent<Text>().text = "������";     // �������� ����� ������������ ������ �� "������"
        deathText.GetComponent<Text>().color = Color.yellow;// ��������� ����� "������" � ������ (���� �������)

        scoreBoardText.GetComponent<Text>().text = score.ToString();    // �������� � ����� ���������� ��������� �����

        if(score > Score_Board_Stat_Scr.highScore)
        {                        // ���� ���������� ����� � ���� ���� ������, ��� ������ ���������
            Score_Board_Stat_Scr.highScore = score;                         // �������� ����� ������ ���������
            congratilateText.SetActive(true);                               // ������������ ��������������� ������� 
            scoreBoardText.GetComponent<Text>().color = Color.yellow;       // ��������� ������� ����� � ������ (���� ������)
        }
        Time.timeScale = 0;             // ���������� ����� (��������� ���� �� �����)
    }

    public void OPEN_AD_FORHP_ANSWER (string b)
    {

        if(b == "YES")
        {
            Time.timeScale = 0;
            if(adForHPAnswerBool == false)
            {
                deathCanvas.gameObject.SetActive(true);
            }
            adForHPAnswerBool = true;
        }
        else if(b == "NO")
        {
            PLAYER_DESTROY(mainCamera.GetComponent<Enemy_Spawner_Scr>().score);
            playerGO.GetComponent<PlayerHealth>().adBool = true;
        }
    }
}