using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActive : EventTriggerFunction
{
    public GameObject ItemToShow;

    public override void Enter()
    {
        if (Trigger)
        {
            ItemToShow.SetActive(true);
            this.enabled = false;
        }
    }
}
