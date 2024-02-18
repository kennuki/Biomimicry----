using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollSwoopTrigger : EventTriggerFunction
{
    public RightTargetInRoom rightTarget;
    public DollSwoop swoop;
    public Collider prison;
    public override void Enter()
    {
        if (Trigger)
        {
            if(rightTarget.inRoom == false)
            {
                swoop.enabled = true;
                prison.enabled = true;
            }
        }
    }
}
    

