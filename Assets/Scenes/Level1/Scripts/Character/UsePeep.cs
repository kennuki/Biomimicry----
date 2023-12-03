using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePeep : MonoBehaviour
{
    public GameObject Black;
    private Material material;
    private float TargetAlpha = 0.85f;
    public PeepMaterial peepMaterial;
    public bool useAllow = false;
    private Material[] peep_materials;
    private bool isUsed = false;
    private bool isProcessing = false;
    private void Awake()
    {
        material = Black.GetComponent<Renderer>().material;
        peep_materials = peepMaterial.materials;
        foreach (Material renderer in peep_materials)
        {
            renderer.SetFloat("_Alpha", 0);
        }
    }
    private void Update()
    {
        if (UseDevice() && !isProcessing)
        {
            if (!isUsed)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                    ActiveDevice();
            }
            else if (isUsed)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                    InactiveDevice();
            }
        }

    }
    private void ActiveDevice()
    {
        Black.SetActive(true);
        StartCoroutine(Fadein());
    }
    private void InactiveDevice()
    {
        StartCoroutine(Fadeout());
    }
    private IEnumerator Fadein()
    {
        isProcessing = true;
        for (float i = 0; i < TargetAlpha; i+=Time.deltaTime)
        {
            Color Target_color = Color.black;
            Target_color.a = i;
            material.SetColor("_BaseColor", Target_color);

            foreach (Material material in peep_materials)
            {
                material.SetFloat("_Alpha", i*(1/TargetAlpha));
            }
            yield return null;
        }
        isUsed = true;
        isProcessing = false;
    }
    private IEnumerator Fadeout()
    {
        isProcessing = true;
        for (float i = TargetAlpha; i > 0; i -= Time.deltaTime)
        {
            Color Target_color = Color.black;
            Target_color.a = i;
            material.SetColor("_BaseColor", Target_color);

            foreach (Material material in peep_materials)
            {
                material.SetFloat("_Alpha", i * (1 / TargetAlpha));
            }
            yield return null;
        }
        isUsed = false;
        isProcessing = false;
        Black.SetActive(false);
    }
    private bool UseDevice()
    {
        if (!useAllow)
            return false;
        if (Character.ActionProhibit)
            return false;
        if (Character.AllProhibit)
            return false;
        return true;
    }
}
