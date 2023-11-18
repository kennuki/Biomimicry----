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
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("0");
        if (other.name == "Character")
        {
            Trigger = true;
            other.enabled = false;
        }
    }
    public abstract void Enter();
    public void Update()
    {
        Enter();
    }
}
