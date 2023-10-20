using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosRotAdjust : MonoBehaviour
{
    public bool arrive = false;
    public Vector3 AdjustPosition;
    public Vector3 AdjustAngle;
    Transform LeftArm;
    Transform character;
    private void Start()
    {
        character = GameObject.Find("Character").GetComponent<Transform>();
        LeftArm = character.GetComponent<Character>().Left_Hand.transform;
        characterController = character.gameObject.GetComponent<CharacterController>();
        StartCoroutine(MoveToTargetPosition());
    }

    float Distance;
    private void Update()
    {
        Distance = Vector2.Distance(new Vector2(character.position.x, character.position.z), (new Vector2(AdjustPosition.x, AdjustPosition.z)));
    }
    

    private IEnumerator Arm_Adjust2()
    {
        Vector3 CharacerToRod = character.position - transform.position;
        CharacerToRod = new Vector3(CharacerToRod.x, 0, CharacerToRod.z);
        Vector3 RodForward = transform.rotation * Vector3.forward;
        RodForward = new Vector3(RodForward.x, 0, RodForward.z);
        float rotationAngle = Vector3.SignedAngle(RodForward, CharacerToRod, Vector3.up);
        Quaternion rotationQuaternion = Quaternion.AngleAxis(rotationAngle, Vector3.up);
        Quaternion CharacterRotationDifference = Quaternion.Euler(0, AdjustAngle.y, 0) * Quaternion.Inverse(character.rotation);
        Quaternion TargetRotation = character.rotation * rotationQuaternion * CharacterRotationDifference;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            character.rotation = Quaternion.Lerp(character.rotation, TargetRotation, 0.7f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        this.enabled = false;
        yield break;


    }
    private IEnumerator Arm_Adjust1()
    {
        Vector3 OriginAngle = LeftArm.localEulerAngles;
        float Variation = AdjustAngle.z - OriginAngle.x;
        Quaternion OriginRotation = LeftArm.localRotation;
        Quaternion OriginRotation2 = LeftArm.rotation;
        LeftArm.Rotate(character.rotation * new Vector3(0, 0, -Variation), Space.World);
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
    private CharacterController characterController;
    private IEnumerator MoveToTargetPosition()
    {
        yield return null;
        while (Distance > 0.1f)
        {
            Vector3 direction = (AdjustPosition - character.position).normalized;
            Vector3 movement = new Vector3(direction.x,0,direction.z) * 2 * Time.deltaTime;
            Character.AllProhibit = true;
            characterController.Move(movement);
            yield return null;
        }
        arrive = true;
        Character.AllProhibit = false;
        StartCoroutine(Arm_Adjust1());
        StartCoroutine(Arm_Adjust2());
        yield break;
    }
}
