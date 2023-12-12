using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBoss : MonoBehaviour
{
    public Material render;
    public float alpha=0;
    private void Update()
    {
        render.SetFloat("_Alpha",alpha);
    }
}
