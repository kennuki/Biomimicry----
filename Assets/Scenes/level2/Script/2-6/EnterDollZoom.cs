using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDollZoom : EventTriggerFunction
{
    public DollSwoop dollSwoop;
    public override void Enter()
    {
        if (Trigger)
        {
            dollSwoop.enabled = true;
        }
    }
}
