using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    public GameObject chair;
    public EventActive active;
    public PosRotAdjust adjust;
    public StageRoutine routine;
    private void Update()
    {
        if (active.Active)
        {
            WhenActive();
        }
        if (adjust.arrive)
        {
            active.ContinuePlayerAction = true;
            this.enabled = false;
        }
    }

    private void WhenActive()
    {
        if (chair != null)
            chair.SetActive(true);
        adjust.enabled = true;
        routine.enabled = true;
    }
}
