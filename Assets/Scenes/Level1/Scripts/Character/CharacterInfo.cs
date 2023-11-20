using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public bool AllProhibit = false;
    public bool ActionProhibit = false;
    public bool cameraRotate = true;
    public bool control = true;
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
        character.position = CharacterPos;
        character.localEulerAngles = CharacterEulerAngle;
        if (control)
        {
            controller.enabled = true;
        }
        else
        {
            controller.enabled = false;
        }
    }
}
