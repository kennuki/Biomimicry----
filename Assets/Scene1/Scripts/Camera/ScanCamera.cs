using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCamera : MonoBehaviour
{
    private Transform Character;
    private void Start()
    {
        Character = GameObject.Find("Character").transform;
    }
    private void Update()
    {
        transform.LookAt(Character);
        transform.Rotate(-45, 0, 0);
    }
}
