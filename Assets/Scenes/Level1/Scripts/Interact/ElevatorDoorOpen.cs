using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorOpen : MonoBehaviour
{
    public AudioSource source;
    private EventActive Event;
    private void Start()
    {
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
    public Animator anim;
    private IEnumerator DoorOpen()
    {
        source.Play();
        yield return new WaitForSeconds(0.2f);
        anim.enabled = true;
        anim.SetBool("Open", true);
    }



}
