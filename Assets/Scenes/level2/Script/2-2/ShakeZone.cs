using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeZone : MonoBehaviour
{
    public CameraRotate cameraRotate;
    public float shakeAmplitude = 1;
    private Transform player;
    private float dis;
    private float rate;
    private void Start()
    {
        cameraRotate = GameObject.Find("CameraGroup").transform.Find("CM vcam1").GetComponent<CameraRotate>();
        player = GameObject.Find("Character").transform;
    }
    private void Update()
    {
        dis = Vector3.Distance(transform.position, player.position);
        disToRate();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Character")
        {
            cameraRotate.Amplitude = shakeAmplitude*rate;
            cameraRotate.shake = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Character")
        {
            cameraRotate.shake = false;
        }
    }
    private void disToRate()
    {
        rate = Mathf.Clamp(1/dis, 0, 1);
    }
}
