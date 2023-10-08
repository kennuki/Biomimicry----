using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carton : InteractObjects
{
   [SerializeField] private Rigidbody rb;

    public override void Enter()
    {
        rb.AddForce(impact);
        Debug.Log(impact);
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        
    }

}
