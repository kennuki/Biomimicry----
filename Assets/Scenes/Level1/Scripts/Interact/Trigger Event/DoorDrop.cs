using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorDrop : EventTriggerFunction
{
    public PlayableDirector playableDirector;
    public PlayableAsset yourTimelineAsset;
    public override void Enter()
    {
        if (Trigger)
        {
            playableDirector.playableAsset = yourTimelineAsset;
            playableDirector.Play();
            Trigger = false;
            this.enabled = false;
        }
    }

}
