using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_hand_weight : MonoBehaviour
{
    public Animator anim;
    public float hand_weight = 0;
    private void Update()
    {
        anim.SetLayerWeight(1, hand_weight);
    }
}
