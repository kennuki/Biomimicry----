using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosRotAdjust : MonoBehaviour
{
    public bool arrive = false;
    public Vector3 AdjustPosition;
    public float AdjustAngle = 50;
    Transform LeftArm;
    Transform Character;
    private void Start()
    {
        Character = GameObject.Find("Character").GetComponent<Transform>();
        LeftArm = Character.GetComponent<Character>().Left_Hand.transform;
        characterController = Character.gameObject.GetComponent<CharacterController>();
        StartCoroutine(MoveToTargetPosition());
    }

    float Distance;
    private void Update()
    {
        Distance = Vector2.Distance(new Vector2(Character.position.x, Character.position.z), (new Vector2(AdjustPosition.x, AdjustPosition.z)));
        //Debug.Log(Distance);
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

    private IEnumerator Arm_Adjust2()
    {
        Vector3 CharacerToRod = Character.position - transform.position;
        CharacerToRod = new Vector3(CharacerToRod.x, 0, CharacerToRod.z);
        Vector3 RodForward = transform.rotation * Vector3.forward;
        RodForward = new Vector3(RodForward.x, 0, RodForward.z);
        float rotationAngle = Vector3.SignedAngle(RodForward, CharacerToRod, Vector3.up);
        Quaternion rotationQuaternion = Quaternion.AngleAxis(rotationAngle, Vector3.up);
        Quaternion CharacterRotationDifference = Quaternion.Euler(0, AdjustAngle, 0) * Quaternion.Inverse(Character.rotation);
        Quaternion TargetRotation = Character.rotation * rotationQuaternion * CharacterRotationDifference;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            Character.rotation = Quaternion.Lerp(Character.rotation, TargetRotation, 0.7f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        this.enabled = false;
        yield break;


    }

    private CharacterController characterController;
    private IEnumerator MoveToTargetPosition()
    {
        yield return null;
        while (Distance > 0.1f)
        {
            Vector3 direction = (AdjustPosition - Character.position).normalized;
            Vector3 movement = new Vector3(direction.x,0,direction.z) * 2 * Time.deltaTime;

            characterController.Move(movement);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        arrive = true;
        //StartCoroutine(Arm_Adjust1());
        StartCoroutine(Arm_Adjust2());
        yield break;
    }
}
