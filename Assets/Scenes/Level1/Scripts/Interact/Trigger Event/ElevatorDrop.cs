using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDrop : EventTriggerFunction
{
    public Animator anim;
    public override void Enter()
    {
        if (Trigger)
        {
            anim.enabled = true;
        }
    }
}
