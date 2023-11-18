using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetectPlayer : MonoBehaviour
{
    public int ColorState = 0;
    private void Start()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.name == "Character")
        {
            Floor.FloorState = ColorState;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Character")
        {
            Floor.FloorState = 0;
        }
    }
    private void Update()
    {
        Debug.Log(Floor.FloorState);
    }
}
