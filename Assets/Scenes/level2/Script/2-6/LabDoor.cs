using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoor : MonoBehaviour
{
    public EventActive eventActive;
    public Animator anim;
    public AnimationClip clip;
    private void Update()
    {
        if (eventActive.Active)
        {
            anim.Play(clip.name);
        }
    }
}
