using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAdjust : MonoBehaviour
{
    [System.Serializable]
    public class AdjustAngle
    {
        public float Rotate_Y_Arm;
        public float Rotate_Z_Arm;
        public float Rotate_Y_Character;
    }
    [SerializeField]
    private AdjustAngle adjustAngle;
    public float RecoverTime;

    Transform LeftArm;
    Transform character;
    private void Start()
    {
        character = GameObject.Find("Character").GetComponent<Transform>();
        LeftArm = character.GetComponent<Character>().Left_Hand.transform;
    }
    public void adjust()
    {
        StartCoroutine(Arm_Adjust2());
        StartCoroutine(Arm_Adjust1());
    } 
    private IEnumerator Arm_Adjust2()
    {
        Vector3 CharacerToRod = character.position - transform.position;
        CharacerToRod = new Vector3(CharacerToRod.x, 0, CharacerToRod.z);
        Vector3 RodForward = transform.rotation * Vector3.forward;
        RodForward = new Vector3(RodForward.x, 0, RodForward.z);
        float rotationAngle = Vector3.SignedAngle(RodForward, CharacerToRod, Vector3.up);
        Quaternion rotationQuaternion = Quaternion.AngleAxis(rotationAngle, Vector3.up);
        Quaternion CharacterRotationDifference = Quaternion.Euler(0, adjustAngle.Rotate_Y_Character, 0) * Quaternion.Inverse(character.rotation);
        Quaternion TargetRotation = character.rotation * rotationQuaternion * CharacterRotationDifference;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            character.rotation = Quaternion.Lerp(character.rotation, TargetRotation, 0.7f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        character.rotation = TargetRotation;
        this.enabled = false;
        yield break;


    }
    private IEnumerator Arm_Adjust1()
    {
        Vector3 OriginAngle = LeftArm.localEulerAngles;
        OriginAngle = new Vector3(OriginAngle.x, (character.localEulerAngles.y + 180) % 360 - 180, (OriginAngle.z + 180) % 360 - 180);

        Vector3 Variation = new Vector3(0, adjustAngle.Rotate_Y_Arm - OriginAngle.y, adjustAngle.Rotate_Z_Arm - OriginAngle.z);
        Quaternion OriginRotation = LeftArm.localRotation;
        Quaternion OriginRotation2 = LeftArm.rotation;
        LeftArm.Rotate(character.rotation * new Vector3(0, Variation.y, 0), Space.World);
        LeftArm.Rotate(new Vector3(0, 0, Variation.z), Space.World);
        Quaternion TargetRotation = LeftArm.rotation;
        LeftArm.localRotation = OriginRotation;
        for (float i = 0; i < RecoverTime; i += Time.deltaTime)
        {
            LeftArm.rotation = Quaternion.Lerp(LeftArm.rotation, TargetRotation, 0.5f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (float i = 0; i < 0.8f; i += Time.deltaTime)
        {
            LeftArm.localRotation = Quaternion.Lerp(LeftArm.localRotation, OriginRotation, 0.1f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        LeftArm.localRotation = OriginRotation;
        yield break;
    }   
}
