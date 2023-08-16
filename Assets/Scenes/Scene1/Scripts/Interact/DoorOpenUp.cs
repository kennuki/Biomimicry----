using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenUp : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            anim.enabled = true;
        }
        this.enabled = false;
    }
}
