using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = GameObject.Find("Character").transform;
    }
    void Update()
    {
        transform.LookAt(target);
    }
}
