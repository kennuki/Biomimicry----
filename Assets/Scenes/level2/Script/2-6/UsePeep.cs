using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePeep : MonoBehaviour
{
    public GameObject Black;
    public DialogManager manager;
    private Material material;
    public ParticleSystem particle;
    private float TargetAlpha = 0.85f;
    public PeepMaterial peepMaterial;
    public bool useAllow = false;
    private Material[] peep_materials;
    private bool isUsed = false;
    private bool isProcessing = false;
    public static bool DeviceUse= false;
    public FlashLight flashLight;
    public AudioSource source;
    public AudioClip[] clip;
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
        source.clip = clip[0];
        source.Play();
        flashLight.TurnOffFlashlight();
        Black.SetActive(true);
        DeviceUse = true;
        StartCoroutine(Fadein());
    }
    private void InactiveDevice()
    {
        DeviceUse = false;
        StartCoroutine(Fadeout());
    }
    private IEnumerator Fadein()
    {
        var colorModule = particle.colorOverLifetime;
        colorModule.color = new ParticleSystem.MinMaxGradient(new Color(1,1,1,0));
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
        StartCoroutine(manager.SetDialog());
        source.clip = clip[1];
        source.Play();
    }
    private IEnumerator Fadeout()
    {
        source.clip = clip[2];
        source.Play();
        isProcessing = true;
        for (float i = TargetAlpha; i > 0; i -= Time.deltaTime)
        {
            Color Target_color = Color.black;
            Color Target_color2 = Color.white;
            Target_color.a = i;
            Target_color2.a = i;
            material.SetColor("_BaseColor", Target_color);
            var colorModule = particle.colorOverLifetime;
            colorModule.color = new ParticleSystem.MinMaxGradient(Target_color2);
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
