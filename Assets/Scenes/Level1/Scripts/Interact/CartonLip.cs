using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartonLip : MonoBehaviour
{
    public float gravityStrength = 9.8f;

    private Rigidbody rb;

    [SerializeField] private float LipAngle = 40;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Quaternion rotation = transform.parent.rotation;

        Vector3 localGravity = rotation*Quaternion.Euler(0,0, LipAngle) * Vector3.down * gravityStrength;

        rb.AddForce(localGravity, ForceMode.Acceleration);
    }
}
