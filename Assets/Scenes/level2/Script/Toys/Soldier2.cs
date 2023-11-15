using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier2 : MonoBehaviour
{
    public Transform target; 
    public float orbitSpeed = 5f;
    private Vector3 PosCache;
    private void Start()
    {
        PosCache = transform.position;
    }
    void Update()
    {
        if (target != null)
        {
            transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
        Vector3 moveDirection = PosCache - transform.position;
        if (moveDirection != Vector3.zero)
        {
            transform.LookAt(transform.position - moveDirection);
            PosCache = transform.position;
        }

    }
}
