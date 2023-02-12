using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisArea_Scr: MonoBehaviour {

    public Player_Scr playerScr;      // —сылка на родител€       

    public void OnTriggerEnter2D(Collider2D goalEnter) {
        if(goalEnter.gameObject.layer == 7) {
            playerScr.GoalsList.Add(goalEnter.gameObject);

            }
        }

    public void OnTriggerExit2D(Collider2D goalExit) {
        if(goalExit.gameObject.layer == 7) {
            playerScr.GoalsList.Remove(goalExit.gameObject);
            }
        }
    }
