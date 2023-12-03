using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public Animator anim;
    public bool Trigger = false;
    private void Update()
    {
        if (Trigger)
        {
            TalkAnim();
            Trigger = false;
        }
    }
    private void TalkAnim()
    {
        anim.SetInteger("Talk", 1);
        anim.SetInteger("Walk", 0);
    }
}
