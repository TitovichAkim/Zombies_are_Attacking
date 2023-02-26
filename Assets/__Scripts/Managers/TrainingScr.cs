using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScr : MonoBehaviour
{
    public GameObject       trainingCanvas;

    public void Start ()
        {
        if (SaveStatic.trainingBoolParam == 1)
            {
            Time.timeScale = 0;
            } else
            {
            trainingCanvas.SetActive(false);
            }
        }

    public void CloseTrainingCanvas ()
        {
        Time.timeScale = 1;
        trainingCanvas.SetActive(false);
        }
    }
