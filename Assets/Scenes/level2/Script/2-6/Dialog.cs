using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public Color[] color;
    public float[] ShaderGlow;
    public float[] DistortSpeed;
    public float[] Alpha;
    public Material target_material;
    public ParticleSystem particle;
    private Color default_color = new Color(1, 1, 1, 1);
    private float default_ShaderGlow = 0.7f;
    private float default_DistortSpeed = 0.02f;
    private float default_Alpha = 0.95f;
    public float textSpeed;
    public GameObject DialogFrame;
    private int index;
    private bool typing = false;
    private float rate = 1;
    public bool Choose = false;
    public DialogAsset[] ExtendDialog;
    void Start()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (typing)
            rate = 1.5f;
        else
        {
            rate = 1;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == lines[index])
            {
                SpeedSet(DistortSpeed[index]);
                GlowSet(ShaderGlow[index]);
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            //ShaderToDefault();
            gameObject.SetActive(false);
            DialogFrame.SetActive(false);
        }
    }
    public IEnumerator startDialog()
    {
        textComponent.text = string.Empty;
        //ShaderToDefault();
        index = 0;
        yield return new WaitForSeconds(1);
        StartCoroutine(TypeLine());
    }
    private IEnumerator TypeLine()
    {
        StartCoroutine(SpeedFadein(DistortSpeed[index],rate));
        StartCoroutine(ColorFadein(color[index]));
        StartCoroutine(AlphaFadein(Alpha[index]));
        StartCoroutine(GlowPowerFadein(ShaderGlow[index],rate));
        foreach(char c in lines[index].ToCharArray())
        {
            typing = true;
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        typing = false;
        SpeedSet(DistortSpeed[index]);
        GlowSet(ShaderGlow[index]);
    }
    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //ShaderToDefault();
            gameObject.SetActive(false);
            DialogFrame.SetActive(false);
        }
    }
    //_Distort_Speed  //GlowPower   //_Alpha
    private void ShaderToDefault()
    {
        var main = particle.main;
        main.startColor = new ParticleSystem.MinMaxGradient(default_color);
        textComponent.color = default_color;
        target_material.SetFloat("_Distort_Speed", default_DistortSpeed);
        target_material.SetColor("_Color", default_color);
        target_material.SetFloat("_GlowPower", default_ShaderGlow);
        StartCoroutine(AlphaFadein(default_Alpha));
    }
    private IEnumerator AlphaFadein(float target)
    {
        float start_alpha = target_material.GetFloat("_Alpha");
        for (float i= start_alpha; i < target; i += Time.deltaTime * 2)
        {
            target_material.SetFloat("_Alpha", i);
            yield return null;
            yield return null;
        }
    }
    private IEnumerator SpeedFadein(float target,float speed)
    {
        float start_ = target_material.GetFloat("_Distort_Speed")*speed;
        for (float i = start_; i < target; i += Time.deltaTime)
        {
            target_material.SetFloat("_Distort_Speed", i);
            yield return null;
        }
    }
    private IEnumerator GlowPowerFadein(float target,float speed)
    {
        float start_ = target_material.GetFloat("_GlowPower")*speed;
        for (float i = start_; i < target; i += Time.deltaTime)
        {
            target_material.SetFloat("_GlowPower", i);
            yield return null;
        }
    }
    private IEnumerator ColorFadein(Color target)
    {
        Color start_ = target_material.GetColor("_Color");
        Color difference = (target - start_);
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            Color target_color = start_ + difference * i;
            target_material.SetColor("_Color", target_color);
            var main = particle.main;
            main.startColor = new ParticleSystem.MinMaxGradient(target_color);
            textComponent.color = target_color;
            yield return null;
        }
    }
    private void SpeedSet(float target)
    {
        target_material.SetFloat("_Distort_Speed", target);
    }
    private void GlowSet(float target)
    {
        target_material.SetFloat("_GlowPower", target);
    }
}
