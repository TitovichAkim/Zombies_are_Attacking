using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScr : MonoBehaviour {

    public Toggle       trainingTogle;          // Отвечает за включение и выключение обучения в игре


    public void Start() {

        }
    // Считывает из файла и заполняет ранее настроенные параметры при открытии меню
    public void readAndWriteParameters() {
        if(SaveStatic.trainingBoolParam == 1) {
            trainingTogle.isOn = true;
            } else {
            trainingTogle.isOn = false;
            }       // Загрузка состояния обучения
        }

    // Считывает все, что введено на данный момент в настройках и сохраняет это
    public void readAndSaveParameters() {
        if(trainingTogle.isOn == true) {
            SaveStatic.trainingBoolParam = 1;
            } else {
            SaveStatic.trainingBoolParam = 0;
            }              // Сохраненеие состояния обучения

        }
}
