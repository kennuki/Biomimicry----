using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EventTriggerFunction : MonoBehaviour
{
    public Transform Character;
    private void Start()
    {
        Character = GameObject.Find("Character").transform;
    }


    public bool Trigger = false;
    public void OnTriggerStay(Collider other)
    {
        if(other.name == "Character")
        {
            Trigger = true;
        }
    }
    public abstract void Enter();
}
