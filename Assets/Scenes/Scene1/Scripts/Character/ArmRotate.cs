using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotate : MonoBehaviour
{
    float OriginAngle = 0;
    float Variation;
    public Transform PlayerCamera;
    public Transform Character;
    public float RotateRate = 1;
    private void Start()
    {
        OriginAngle = PlayerCamera.eulerAngles.x;
        if (OriginAngle < -180)
        {
            OriginAngle += 360;
        }
        if (OriginAngle > 180)
        {
            OriginAngle -= 360;
        }
    }
    private void Update()
    {
        float TargetAngle = PlayerCamera.eulerAngles.x;
        if (TargetAngle < -180)
        {
            TargetAngle += 360;
        }
        if (TargetAngle > 180)
        {
            TargetAngle -= 360;
        }
        Variation = TargetAngle - OriginAngle;
        transform.Rotate(Character.rotation* new Vector3(0, 0, Variation * RotateRate), Space.World);
        OriginAngle = TargetAngle;
        Vector3 TargetRotation = new Vector3(transform.localEulerAngles.x , transform.localEulerAngles.y, -PlayerCamera.eulerAngles.x);
        //Vector3 TargetRotation = new Vector3(-PlayerCamera.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 OriginRotation = transform.localEulerAngles;

        if (TargetRotation.z < -180)
        {
            TargetRotation.z += 360;
        }
        if (OriginRotation.z > 180)
        {
            OriginRotation.z -= 360;
        }
        //transform.localEulerAngles = Vector3.Lerp(OriginRotation, TargetRotation, 0.2f);
    }
}