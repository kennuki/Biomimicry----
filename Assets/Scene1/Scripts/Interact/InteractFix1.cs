using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFix1 : MonoBehaviour
{
    public static float maxspeed = 0;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(maxspeed == 0)
        {
            this.gameObject.isStatic = true;
        }
        else
        {
            this.gameObject.isStatic = false;
        }
        if (rb.velocity.magnitude > maxspeed)
        {
            rb.velocity = rb.velocity.normalized * maxspeed;
        }
    }
}
