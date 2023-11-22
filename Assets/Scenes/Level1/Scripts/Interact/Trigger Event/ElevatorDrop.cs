using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorDrop : EventTriggerFunction
{
    private GameObject Player;
    public PlayableDirector playableDirector;
    public PlayableAsset yourTimelineAsset;
    public override void Enter()
    {
        Player = GameObject.Find("Character");
        if (Trigger)
        {
            
            playableDirector.playableAsset = yourTimelineAsset;
            playableDirector.Play();
            Trigger = false;
            this.enabled = false;
        }
    }
}
