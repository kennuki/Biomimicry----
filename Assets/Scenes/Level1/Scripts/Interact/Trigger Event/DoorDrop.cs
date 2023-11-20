using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorDrop : EventTriggerFunction
{
    public PlayableDirector playableDirector;
    public override void Enter()
    {
        if (Trigger)
        {
            playableDirector.Play();
            Trigger = false;
            this.enabled = false;
        }
    }

}
