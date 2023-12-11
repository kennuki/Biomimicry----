using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Timeline;

public class DoorDrop : EventTriggerFunction
{
    public PlayableDirector playableDirector;
    public PlayableAsset yourTimelineAsset;
    public string targetTrackName1, targetTrackName2;
    private GameObject Door,redLight;
    //public TrackAsset trackToRemove;
    private void Awake()
    {
        Door = GameObject.Find("Gate").transform.Find("LiftGate").gameObject;
        redLight = GameObject.Find("Warning Light");
        playableDirector = GameObject.Find("Timeline").GetComponent<PlayableDirector>();



    }
    public override void Enter()
    {
        if (Trigger)
        {
            playableDirector.playableAsset = yourTimelineAsset;
            var timelineAsset = (TimelineAsset)playableDirector.playableAsset;
            var trackList = timelineAsset.GetOutputTracks();
            foreach (var track in trackList)
            {
                if (track.name == targetTrackName1)
                {
                    playableDirector.SetGenericBinding(track, Door);
                    break;
                }
            }
            foreach (var track in trackList)
            {
                if (track.name == targetTrackName2)
                {
                    playableDirector.SetGenericBinding(track, redLight);
                    break;
                }
            }
            playableDirector.Play();
            Trigger = false;
            this.enabled = false;
        }
    }
}
