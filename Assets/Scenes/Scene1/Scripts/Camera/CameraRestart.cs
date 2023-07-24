using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRestart : MonoBehaviour
{
    public CameraRotate CameraRotate;
    void Start()
    {
        CameraRotate.enabled = true;
    }

}
