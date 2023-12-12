using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAsset : MonoBehaviour
{
    public Material material;
    public string[] lines;
    public Color[] color;
    public float[] ShaderGlow;
    public float[] DistortSpeed;
    public float[] Alpha;
    public bool Choose = false;
    public ChooseDialogAsset chooseDialog;
    public Material dialog_frame;
    public AudioSource source;
    public AudioClip[] clip;
    private void Awake()
    {
        material.SetColor("_Color", new Color(1, 1, 1, 1));
        material.SetFloat("_Alpha", 0);
    }

}
