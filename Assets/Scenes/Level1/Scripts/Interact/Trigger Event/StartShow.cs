using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShow : MonoBehaviour
{
    public EventActive active;
    public MoveToTarget move;
    private void Update()
    {
        if (active.Active)
        {
            move.enabled = true;
            this.enabled = false;
        }

    }
}
