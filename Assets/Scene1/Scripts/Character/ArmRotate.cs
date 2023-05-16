using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotate : MonoBehaviour
{
    public Transform PlayerCamera;
    private void Update()
    {
        Vector3 TargetRotation = new Vector3(-PlayerCamera.eulerAngles.x , transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 OriginRotation = transform.eulerAngles;

        if (TargetRotation.x < -180)
        {
            TargetRotation.x += 360;
        }
        if (OriginRotation.x > 180)
        {
            OriginRotation.x -= 360;
        }
        transform.eulerAngles = Vector3.Lerp(OriginRotation, TargetRotation, 0.2f);
    }
}