using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdViewTrigger : EventTriggerFunction
{
    private CinemachineVirtualCamera Third_Camera;
    private void Start()
    {
        Third_Camera = GameObject.Find("CameraGroup").transform.Find("CM vcam2").GetComponent<CinemachineVirtualCamera>();
    }
    public override void Enter()
    {
        if (Trigger)
        {
            Third_Camera.Priority = 11;
            int layerIndex = LayerMask.NameToLayer("Character");
            Camera.main.cullingMask |= 1 << layerIndex;
            this.enabled = false;
        }
    }
}
