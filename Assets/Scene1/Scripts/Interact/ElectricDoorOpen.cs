using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ElectricDoorOpen : MonoBehaviour
{
    public CinemachineVirtualCamera Rod_Camera;
    public Animator Switch;
    public GameObject door;
    Transform LookPoint;
    Transform RightArm;
    private CameraRotate CameraRotate;
    void Start()
    {
        CameraRotate = GameObject.Find("CM vcam1").GetComponent<CameraRotate>();
        LookPoint = GameObject.Find("Character").GetComponent<Transform>().Find("CameraLookPoint").GetComponent<Transform>();
        RightArm = LookPoint.Find("ArmR_Idle").GetComponent<Transform>();
        Switch.enabled = true;
        StartCoroutine(Open(1.5f));
        StartCoroutine(ArmR_Adjust());
    }

    
    void Update()
    {


    }
    private IEnumerator Open(float second)
    {
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CameraRotate.CameraShake());
        int layerIndex = LayerMask.NameToLayer("Character"); 
        Camera.main.cullingMask |= 1 << layerIndex;
        Rod_Camera.Priority = 11;
        for (float i = 0; i < second; i += Time.deltaTime)
        {
            door.transform.Translate(0, 2 * Time.deltaTime, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Rod_Camera.Priority = 0;
        yield return new WaitForSeconds(1.4f);
        Camera.main.cullingMask &= ~(1 << layerIndex);
    }
    private IEnumerator ArmR_Adjust()
    {
        Vector3 OriginAngle = RightArm.localEulerAngles;
        Vector3 TargetAngle = RightArm.localEulerAngles;
        TargetAngle.x += LookPoint.eulerAngles.x - (-20);
        if (TargetAngle.x > 180)
        {
            TargetAngle.x -= 360;
        }
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            RightArm.localEulerAngles = Vector3.Lerp(RightArm.localEulerAngles, TargetAngle, 0.1f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            RightArm.localEulerAngles = Vector3.Lerp(RightArm.localEulerAngles, OriginAngle, 0.1f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
