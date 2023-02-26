using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;



public class Saver_Scr : MonoBehaviour {

    public void Awake() {
        if(Score_Board_Stat_Scr.loadParameter == false) {  // Если эта сцена запустилась в момент загрузки игры и файл с сохраненными данными отсутствует
            LOAD_GAME();                                   // При запуске игры прочитать файл с сохранением и загрузить из него самый большой результат
            }
        LOAD_SETTINGS();
        }
    public void Start() {
        if(Score_Board_Stat_Scr.loadParameter != false) {
            SAVE_GAME();
            }
        }
    public void SAVE_GAME() {
        PlayerPrefs.SetInt("HighScore", Score_Board_Stat_Scr.highScore);
        }

    public void LOAD_GAME() {

        Score_Board_Stat_Scr.highScore = PlayerPrefs.GetInt("HighScore");            // Записать значение самого большого результата из файла сохранений
        }

    public void SAVE_SETTINGS() {
        PlayerPrefs.SetInt("Training",SaveStatic.trainingBoolParam);
        }

    public void LOAD_SETTINGS() {
        SaveStatic.trainingBoolParam = PlayerPrefs.GetInt("Training");               // Выгрузить состояние обучения
        }
}
