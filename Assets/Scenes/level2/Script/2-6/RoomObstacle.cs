using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObstacle : MonoBehaviour
{
    public RightTargetInRoom rightTarget;
    Collider cd;
    private void Start()
    {
        cd = this.GetComponent<Collider>();
    }
    void Update()
    {
        if (rightTarget.inRoom)
            cd.enabled = false;
        else
            cd.enabled = true;
    }
}
