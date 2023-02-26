using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Menu_Scr:MonoBehaviour
{
    public GameObject           gameMenuGO;         // Ссылка на панель игрового меню
    public GameObject           deathText;          // Ссылка на текст о смерти
    public GameObject           scoreText;          // Ссылка на текст "Ваши очки"
    public GameObject           scoreBoardText;     // Ссылка на редактируемый текст с цифрами
    public GameObject           congratilateText;   // Ссылка на поздравление, которую показывать, если очки польше, чем последний рекорд
    public GameObject           mainMenuButton;     // Ссылка на кнопку выхода в главное меню после смерти
    public GameObject           mainCamera;         // Ссылка на главную камеру

    public GameObject           playerGO;

    public Canvas               deathCanvas;        // Холст смертельного уменьшения здоровья
    public GameObject           gameOverPanel;      // Панель со всеми элементами результатов смерти
    public GameObject           adHealthPanel;      // Окно вопроса для просмотра рекламы для возрождения
    public bool                 adForHPAnswerBool = false; // Открывалось ли уже это окно

    public GameObject           InputCanvas;        // Ссылки на кнопки, которые нужно скрыть при открытии меню



    public void OPEN_MENU ()
    {           // Вызывается кнопкой открытия меню в правом верхнем углу
        gameMenuGO.SetActive(true);     // Показать игровое меню
        Time.timeScale = 0;             // Остановить время (поставить игру на паузу)
    }
    public void CLOSE_MENU ()
    {          // Кнопка "ПРОДОЛЖИТЬ"
        gameMenuGO.SetActive(false);    // Скрыть игровое меню
        Time.timeScale = 1;             // Возобновить игру (запустить воспроизведение времени)
    }
    public void NEW_GAME_BUTTON ()
    {     // Вызывается кнопкой "НОВАЯ ИГРА"
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Загрузить эту сцену с начала
    }
    public void MAIN_MENU_BUTTON ()
    {                // Вызывается кнопкой "ГЛАВНОЕ МЕНЮ"
        SceneManager.LoadScene("0_Main_Menu");      // Загрузить сцену главного меню
    }
    public void EXIT_GAME ()
    {           // Вызывается кнопкой "ВЫХОД ИЗ ИГРЫ"
        Application.Quit();             // Так выглядит команда выхода из игры
    }

    public void PLAYER_DESTROY (int score)           // Вызывается из скрипта Player при убийстве игрока
    {
        InputCanvas.SetActive(false);                // Убрать отображение 
        deathCanvas.gameObject.SetActive(true);     // Включить холст завершения игры

        scoreBoardText.GetComponent<Text>().text = score.ToString();

        if(score > Score_Board_Stat_Scr.highScore)
        {                       // Если количество очков в этой игре больше, чем лучший результат
            Score_Board_Stat_Scr.highScore = score;                         // Записать новый лучший результат
            congratilateText.SetActive(true);                               // Активировать поздравительную надпись 
            scoreBoardText.GetComponent<Text>().color = Color.yellow;       // Выкрасить надпись числа в желтый (типа золото)
        }
        Time.timeScale = 0;             // Остановить время (поставить игру на паузу)
    }

    public void PLAYER_WIN (int score)
    {             // Вызывается из скрипта Enemy_Spawner_Scr, когда закончилась последння волна
        InputCanvas.SetActive(false);               // Убрать отображение 
        deathCanvas.gameObject.SetActive(true);     // Включить холст завершения игры

        deathText.GetComponent<Text>().text = "ПОБЕДА";     // Изменить текст смертельного текста на "Победа"
        deathText.GetComponent<Text>().color = Color.yellow;// Выкрасить текст "Победа" в желтый (типа золотой)

        scoreBoardText.GetComponent<Text>().text = score.ToString();    // Записать в текст количество набранных очков

        if(score > Score_Board_Stat_Scr.highScore)
        {                        // Если количество очков в этой игре больше, чем лучший результат
            Score_Board_Stat_Scr.highScore = score;                         // Записать новый лучший результат
            congratilateText.SetActive(true);                               // Активировать поздравительную надпись 
            scoreBoardText.GetComponent<Text>().color = Color.yellow;       // Выкрасить надпись числа в желтый (типа золото)
        }
        Time.timeScale = 0;             // Остановить время (поставить игру на паузу)
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