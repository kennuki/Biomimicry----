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
    public string TrackName1, TrackName2, TrackName3, TrackName4;
    private GameObject Door,redLight;
    private GameObject Boss,BossDirty;
    //public TrackAsset trackToRemove;
    private void Awake()
    {
        Door = GameObject.Find("Gate").transform.Find("LiftGate").gameObject;
        redLight = GameObject.Find("Warning Light");
        playableDirector = GameObject.Find("Timeline").GetComponent<PlayableDirector>();
        Boss = GameObject.Find("Boss");
        BossDirty = Boss.transform.Find("Big").transform.Find("BOSS1(dirty)").gameObject;

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
            foreach (var track in trackList)
            {
                if (track.name == TrackName1)
                {
                    playableDirector.SetGenericBinding(track, Boss);
                    break;
                }
            }
            foreach (var track in trackList)
            {
                if (track.name == TrackName2)
                {
                    playableDirector.SetGenericBinding(track, BossDirty);
                    break;
                }
            }
            foreach (var track in trackList)
            {
                if (track.name == TrackName3)
                {
                    playableDirector.SetGenericBinding(track, Boss);
                    break;
                }
            }
            foreach (var track in trackList)
            {
                if (track.name == TrackName4)
                {
                    playableDirector.SetGenericBinding(track, BossDirty);
                    break;
                }
            }
            playableDirector.Play();
            Trigger = false;
            this.enabled = false;
        }
    }
}
