using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ElevatorDrop : EventTriggerFunction
{
    private GameObject Player;
    private PlayableDirector playableDirector;
    public PlayableAsset yourTimelineAsset;
    public string targetTrackName1, targetTrackName2;
    private GameObject elevator;
    private GameObject elevator_door;
    private void Awake()
    {
        elevator = GameObject.Find("Elevator");
        elevator_door = elevator.transform.Find("Door").gameObject;
        playableDirector = GameObject.Find("Timeline").GetComponent<PlayableDirector>();      
    }
    public override void Enter()
    {
        Player = GameObject.Find("Character");
        if (Trigger)
        {
            
            playableDirector.playableAsset = yourTimelineAsset;
            var timelineAsset = (TimelineAsset)playableDirector.playableAsset;
            var trackList = timelineAsset.GetOutputTracks();
            foreach (var track in trackList)
            {
                if (track.name == targetTrackName1)
                {
                    playableDirector.SetGenericBinding(track, elevator);
                    break;
                }
            }
            foreach (var track in trackList)
            {
                if (track.name == targetTrackName2)
                {
                    playableDirector.SetGenericBinding(track, elevator_door);
                    break;
                }
            }

            playableDirector.Play();
            Trigger = false;
            this.enabled = false;
        }
    }
}
