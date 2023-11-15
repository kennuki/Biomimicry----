using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotate : MonoBehaviour
{

    void Start()
    {

    }
    void FixedUpdate()
    {
        RotateFunction();
    }

    float faceAngle;
    Quaternion targetRotation;
    public void RotateFunction()
    {

        float h = Input.GetAxis("Horizontal");
        float j = Input.GetAxis("Vertical");
        faceAngle = Mathf.Atan2(h, j) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, faceAngle + 5 + Character.imaangle * Mathf.Rad2Deg, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f);



    }
}
