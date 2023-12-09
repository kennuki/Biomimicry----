using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Transform target;
    public Transform lookat;
    private Vector3 OriginTarget;
    private Vector3 Originlookat;
    private void Start()
    {
        target = GameObject.Find("Character").transform;
        OriginTarget = target.position;
        Originlookat = lookat.position;
    }

    float AngleDifference;
    void Update()
    {
        if(target == null)
            target = GameObject.Find("Character").transform;
        AngleDifference = (target.eulerAngles.y - lookat.eulerAngles.y)-90;
        if (AngleDifference > 180)
        {
            AngleDifference -= 360;
        }
        if (AngleDifference < -180)
        {
            AngleDifference += 360;
        }
        lookat.LookAt(new Vector3(target.position.x, Originlookat.y, target.position.z));
        if ((Mathf.Abs(AngleDifference) < 110))
        {

            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
    }
}
