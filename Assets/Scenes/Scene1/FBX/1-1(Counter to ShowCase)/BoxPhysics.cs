using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPhysics : MonoBehaviour
{
    private void Update()
    {
        Quaternion worldRotation = transform.rotation;

        Debug.Log(worldRotation.eulerAngles);
    }
}
