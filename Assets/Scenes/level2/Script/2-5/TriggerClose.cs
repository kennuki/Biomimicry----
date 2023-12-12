using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerClose : EventTriggerFunction
{
    public GameObject ItemToHide;

    public override void Enter()
    {
        if (Trigger)
        {
            ItemToHide.SetActive(false);
            this.enabled = false;
        }
    }
}
