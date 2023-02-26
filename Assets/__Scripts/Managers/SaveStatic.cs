using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class SaveStatic {

    private static int trainingBool;           // Хранит в статике включено ли обучение в игре



    public static int trainingBoolParam {
        get {
            return trainingBool;
            }
        set {
            trainingBool = value;
            }
        }

    }
