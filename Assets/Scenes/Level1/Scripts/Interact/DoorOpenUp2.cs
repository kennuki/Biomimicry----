using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorOpenUp2 : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset asset;
    private AudioSource source;
    public Animator Rod;
    public Animator DoorOpenUp_Anim;
    private EventActive Event;
    private void Start()
    {
        Event = this.GetComponent<EventActive>();
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Event.Active)
        {
            StartCoroutine(DoorOpen());
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
