using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Dialog_bear_trigger : EventTriggerFunction
{
    public PlayableDirector playableDirector;
    public PlayableAsset playableAsset;
    public override void Enter()
    {
        if (Trigger)
        {
            playableDirector.playableAsset = playableAsset;
            playableDirector.Play();
            Trigger = false;
            this.enabled = false;
        }
    }
}
