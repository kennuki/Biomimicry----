using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorOpenUp2 : MonoBehaviour
{
    private PlayableDirector director;
    public PlayableAsset asset;
    public Animator Rod;
    public Animator DoorOpenUp_Anim;
    private EventActive Event;
    private void Start()
    {
        director = GameObject.Find("Timeline").GetComponent<PlayableDirector>();
        Event = this.GetComponent<EventActive>();
    }
    private void Update()
    {
        if (Event.Active)
        {
            StartCoroutine(DoorOpen());
            Event.Active = false;
        }
    }
    private IEnumerator DoorOpen()
    {
        director.playableAsset = asset;
        director.Play();
        Rod.enabled = true;
        yield return new WaitForSeconds(1);
        DoorOpenUp_Anim.enabled = true;
    }
}
