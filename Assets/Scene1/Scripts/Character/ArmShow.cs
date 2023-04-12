using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmShow : MonoBehaviour
{
    public Animator anim;
    public Renderer render;
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && anim.GetInteger("HandState") == 0)
        {
            render.material.SetColor("Color", new Vector4(1, 1, 1, 0));
            //SkinnedMesh.enabled = false;
        }
        else
        {
            //SkinnedMesh.enabled = true;
        }
    }
}
