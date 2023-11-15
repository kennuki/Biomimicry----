using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotate2 : MonoBehaviour
{
    public Transform PlayerCamera;
    private void Update()
    {
        Vector3 TargetRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -PlayerCamera.eulerAngles.x-90);
        Vector3 OriginRotation = transform.eulerAngles;

        if (TargetRotation.z < -180)
        {
            TargetRotation.z += 360;
        }
        if (OriginRotation.z > 180)
        {
            OriginRotation.z -= 360;
        }
        transform.eulerAngles = Vector3.Lerp(OriginRotation, TargetRotation, 0.2f);
    }
}
