using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScr : MonoBehaviour {

    public Toggle       trainingTogle;          // �������� �� ��������� � ���������� �������� � ����


    public void Start() {

        }
    // ��������� �� ����� � ��������� ����� ����������� ��������� ��� �������� ����
    public void readAndWriteParameters() {
        if(SaveStatic.trainingBoolParam == 1) {
            trainingTogle.isOn = true;
            } else {
            trainingTogle.isOn = false;
            }       // �������� ��������� ��������
        }

    // ��������� ���, ��� ������� �� ������ ������ � ���������� � ��������� ���
    public void readAndSaveParameters() {
        if(trainingTogle.isOn == true) {
            SaveStatic.trainingBoolParam = 1;
            } else {
            SaveStatic.trainingBoolParam = 0;
            }              // ����������� ��������� ��������

        }
}
