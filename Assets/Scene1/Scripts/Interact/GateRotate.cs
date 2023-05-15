using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRotate : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 relativeForce = new Vector3(4f,0,0);
    public float AngleBias = 0;
    private Vector3 OriginPos;
    private void Start()
    {
        OriginPos = transform.position;
    }
    private void Update()
    {
        transform.position = OriginPos;
        float angle = transform.localEulerAngles.y;
        if (angle > 180)
        {
            angle -= 360;
        }
        if (angle > 15)
        {
            rb.AddRelativeForce(relativeForce);
        }
        else if (angle > 2)
        {
            rb.AddRelativeForce(relativeForce*0.2f);
        }
        else if(angle<-2)
        {
            rb.AddRelativeForce(relativeForce * -0.1f);
        }
    }
}
