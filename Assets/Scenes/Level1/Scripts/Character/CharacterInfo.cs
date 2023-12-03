using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public bool AllProhibit = false;
    public bool ActionProhibit = false;
    public bool cameraRotate = true;
    public bool moveOnly = false;
    public bool control = true;
    public bool control_by_other = false;
    public bool cancel_thirdview = false;
    public Vector3 CharacterPos;
    public Vector3 CharacterEulerAngle;
    public Transform character;
    private CharacterController controller;
    private void Start()
    {
        controller = character.GetComponent<CharacterController>();
    }
    private void Update()
    {
        Character.AllProhibit = AllProhibit;
        Character.ActionProhibit = ActionProhibit;
        CameraRotate.cameratotate = cameraRotate;
        Character.MoveOnly = moveOnly;
        if (control)
        {
            controller.enabled = true;
        }
        else
        {
            controller.enabled = false;
            if (!control_by_other)
            {
                character.position = CharacterPos;
                character.localEulerAngles = CharacterEulerAngle;
            }

        }
        if (cancel_thirdview)
        {
            Character.LookState = 1;
            cancel_thirdview = false;
        }
    }
}
