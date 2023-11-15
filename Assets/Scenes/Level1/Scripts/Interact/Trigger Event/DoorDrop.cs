using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDrop : EventTriggerFunction
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
