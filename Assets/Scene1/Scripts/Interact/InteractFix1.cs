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
        if(rb.velocity.magnitude == 0)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = false;
        }

    }
}
