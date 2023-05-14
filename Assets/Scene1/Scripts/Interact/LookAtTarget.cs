using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Transform target;
    public Transform lookat;
    private Vector3 OriginTarget;
    private void Start()
    {
        target = GameObject.Find("Character").transform;
        OriginTarget = target.position;
    }

    float AngleDifference;
    void Update()
    {
        AngleDifference = target.eulerAngles.y - lookat.eulerAngles.y;
        if (AngleDifference > 180)
        {
            AngleDifference -= 360;
        }
        lookat.LookAt(new Vector3(target.position.x,OriginTarget.y, target.position.z));
        if ((Mathf.Abs(AngleDifference) < 110))
        {

            transform.LookAt(new Vector3(target.position.x, OriginTarget.y, target.position.z));
        }
    }
}
