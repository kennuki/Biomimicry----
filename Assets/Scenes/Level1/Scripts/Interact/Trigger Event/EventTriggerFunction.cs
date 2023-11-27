using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EventTriggerFunction : MonoBehaviour
{
    public Transform character;
    private void Start()
    {
        character = GameObject.Find("Character").transform;
    }


    public bool Trigger = false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Character")
        {
            Trigger = true;
        }
    }
    public abstract void Enter();
    public void Update()
    {
        Enter();
    }
}
