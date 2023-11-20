using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorOpen : MonoBehaviour
{
    private EventActive Event;
    private void Start()
    {
        Event = this.GetComponent<EventActive>();
    }
    private void Update()
    {
        if (Event.Active)
        {
            DoorOpen();
        }
    }
    public Animator anim;
    private void DoorOpen()
    {
        anim.enabled = true;
        anim.SetBool("Open", true);
    }



}
