using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ElectricDoorOpen : MonoBehaviour
{
    public Animator Rod;
    public Animator DoorOpenUp_Anim;
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
        }
    }
    private IEnumerator DoorOpen()
    {
        Rod.enabled = true;
        yield return new WaitForSeconds(1);
        DoorOpenUp_Anim.enabled = true;
    }
}
