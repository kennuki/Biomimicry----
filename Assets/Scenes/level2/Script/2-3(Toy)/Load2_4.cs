using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Load2_4 : EventTriggerFunction
{
    public PlayableDirector playableDirector;
    public override void Enter()
    {
        if (Trigger)
        {
            Trigger = false;
            playableDirector.Play();
            this.enabled = false;
        }

    }
}
