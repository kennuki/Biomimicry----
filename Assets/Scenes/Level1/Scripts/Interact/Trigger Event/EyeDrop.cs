using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDrop : MonoBehaviour
{
    private Transform Character;
    private void Start()
    {
        Character = GameObject.Find("Character").transform;
    }

    private bool Trigger = false;
    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Character")
        {
            Trigger = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.name == "Character")
        {
            Trigger = false;
        }
    }
    private void Update()
    {
        if (Trigger)
        {
            if(Character.transform.eulerAngles.y<TriggerRange_Angle.y&& Character.transform.eulerAngles.y > TriggerRange_Angle.x)
            {
                EyeDropFunction(Delay);
            }
        }
    }

    public Animator anim;
    public Vector2 TriggerRange_Angle;
    public float Delay = 0;
    private float counter = 0;
    private void EyeDropFunction(float time)
    {
        counter += Time.deltaTime;
        if (counter > Delay)
        {
            anim.enabled = true;
        }
        if(counter > Delay + 1)
        {
            this.enabled = false;
        }
    }
}
