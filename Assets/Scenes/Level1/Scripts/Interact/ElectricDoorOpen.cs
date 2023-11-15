using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ElectricDoorOpen : MonoBehaviour
{
    public CinemachineVirtualCamera Rod_Camera;
    public Animator Switch;
    public GameObject door;
    Transform LeftArm;
    Transform Character;
    private CameraRotate CameraRotate;
    void Start()
    {
        CameraRotate = GameObject.Find("CM vcam1").GetComponent<CameraRotate>();
        Character = GameObject.Find("Character").GetComponent<Transform>();
        LeftArm = Character.GetComponent<Character>().Left_Hand.transform;
        RodBoard = transform.parent;
        Switch.enabled = true;
        StartCoroutine(Open(1.4f));
        StartCoroutine(Arm_Adjust2());
        StartCoroutine(Arm_Adjust1());
    }

    
    void Update()
    {

        Debug.DrawLine(RodBoard.position, Character.position, Color.cyan);
        Debug.DrawLine(RodBoard.position, RodBoard.rotation * Vector3.forward, Color.green);

    }
    private IEnumerator Open(float second)
    {
        yield return new WaitForSeconds(1f);
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
        yield return new WaitForSeconds(2f);
        Camera.main.cullingMask &= ~(1 << layerIndex);
    }
    private IEnumerator Arm_Adjust1()
    {
        Vector3 OriginAngle = LeftArm.localEulerAngles;
        float Variation = 50 - OriginAngle.x;
        Quaternion OriginRotation = LeftArm.localRotation;
        Quaternion OriginRotation2 = LeftArm.rotation;
        LeftArm.Rotate(Character.rotation * new Vector3(0, 0, -Variation), Space.World);
        Quaternion TargetRotation = LeftArm.rotation;
        LeftArm.localRotation = OriginRotation;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            LeftArm.rotation = Quaternion.Lerp(LeftArm.rotation, TargetRotation, 0.5f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            LeftArm.rotation = Quaternion.Lerp(LeftArm.rotation, OriginRotation2, 0.5f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        LeftArm.localRotation = OriginRotation;
        yield break;
    }

    Transform RodBoard;
    private IEnumerator Arm_Adjust2()
    {
        Vector3 CharacerToRod = Character.position - RodBoard.position;
        CharacerToRod = new Vector3(CharacerToRod.x, 0, CharacerToRod.z);
        Vector3 RodForward = RodBoard.rotation * Vector3.forward;
        RodForward = new Vector3(RodForward.x, 0, RodForward.z);
        float rotationAngle = Vector3.SignedAngle(RodForward, CharacerToRod, Vector3.up);
        Quaternion rotationQuaternion = Quaternion.AngleAxis(rotationAngle, Vector3.up);
        Quaternion CharacterRotationDifference = Quaternion.Euler(0, -90, 0) * Quaternion.Inverse(Character.rotation);
        Quaternion TargetRotation = Character.rotation * rotationQuaternion*CharacterRotationDifference;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            Character.rotation = Quaternion.Lerp(Character.rotation, TargetRotation, 0.7f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
       
        yield break;


    }
}
