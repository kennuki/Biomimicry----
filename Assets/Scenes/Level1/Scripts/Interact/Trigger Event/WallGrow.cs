using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrow : EventTriggerFunction
{
    public GameObject[] OtherTrigger;
    public GameObject[] Wall_appear;
    public GameObject[] Wall_disappear;
    public override void Enter()
    {
        Trigger = false;
        foreach(GameObject trigger in OtherTrigger)
        {
            trigger.SetActive(false);
        }
        foreach (GameObject wall in Wall_appear)
        {
            wall.SetActive(true);
        }
        foreach (GameObject wall in Wall_disappear)
        {
            wall.SetActive(false);
        }
        this.enabled = false;
    }
}
