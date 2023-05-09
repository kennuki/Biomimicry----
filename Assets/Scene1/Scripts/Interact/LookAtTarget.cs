using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Transform target;
    public Transform lookat;
    private void Start()
    {
        target = GameObject.Find("Character").transform;
    }

    float AngleDifference;
    void Update()
    {
        AngleDifference = target.eulerAngles.y - lookat.eulerAngles.y;
        if (AngleDifference > 180)
        {
            AngleDifference -= 360;
        }
        lookat.LookAt(target);
        if ((Mathf.Abs(AngleDifference) < 110))
        {

            transform.LookAt(target);
        }
    }
}
