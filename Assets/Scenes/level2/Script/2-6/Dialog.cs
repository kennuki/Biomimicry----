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
    public float textSpeed;
    public GameObject DialogFrame;
    private int index;
    private bool typing = false;
    private float rate = 1;
    public bool Choose = false;
    public ChooseDialogAsset chooseDialog;
    private float magnitude = 0;
    private Vector2 accumulation_dir;
    private float degree = 360;
    public ButtonCreator buttonCreator;
    private bool OnChoosing = false;
    public AudioSource source;
    public AudioClip[] clip;
    void Start()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        degree += Random.Range(-2, 2);
        magnitude = DistortSpeed[index]*Time.deltaTime*rate;
        accumulation_dir.x += Mathf.Cos(Mathf.Deg2Rad * degree) * magnitude - Time.deltaTime * 0.1f;
        accumulation_dir.y += Mathf.Sin(Mathf.Deg2Rad * degree) * magnitude - Time.deltaTime * 0.1f;
        target_material.SetFloat("_accumulation_x", accumulation_dir.x);
        target_material.SetFloat("_accumulation_y", accumulation_dir.y);
        //target_material.SetFloat("_accumulation_y", accumulation_dir.y);
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
                source.clip = clip[Random.Range(0, clip.Length)];
                SpeedSet(DistortSpeed[index]);
                GlowSet(ShaderGlow[index]);
                StopAllCoroutines();
                if (!OnChoosing)
                    NextLine();
            }
            else
            {
                source.clip = clip[Random.Range(0, clip.Length)];
                StopAllCoroutines();
                StartCoroutine(DialogFrameFadeout(0.5f));
                StartCoroutine(FpsFadein(1, -20));
                ShaderToTarget();
                textComponent.text = lines[index];
                typing = false;
                source.Stop();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            Exit();
        }
    }
    public void Exit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CameraRotate.cameratotate = true;
        Character.AllProhibit = false;
        Character.ActionProhibit = false;
        source.Stop();
        OnChoosing = false;
        DialogFrame.SetActive(false);
        gameObject.SetActive(false);
        buttonCreator.RemoveObjectsWithSpecificName();
    }
    public IEnumerator startDialog()
    {
        textComponent.text = string.Empty;
        index = 0;
        yield return null;
        StartCoroutine(TypeLine());
        yield break;
    }
    private IEnumerator TypeLine()
    {
        StartCoroutine(SpeedFadein(DistortSpeed[index],rate));
        StartCoroutine(FpsFadein(1,40));
        StartCoroutine(ColorFadein(color[index]));
        StartCoroutine(AlphaFadein(Alpha[index]));
        StartCoroutine(GlowPowerFadein(ShaderGlow[index],rate));
        source.clip = clip[Random.Range(0, clip.Length)];
        foreach(char c in lines[index].ToCharArray())
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
            typing = true;
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        source.Stop();
        typing = false;
        StartCoroutine(DialogFrameFadeout(0.5f));
        StartCoroutine(FpsFadein(1, -20));
        SpeedSet(DistortSpeed[index]);
        GlowSet(ShaderGlow[index]);
        if(Choose&&index == lines.Length)
        {
            StartChoose();
        }
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
            if (Choose)
            {
                StartChoose();
            }
            else if (!OnChoosing)
                Exit();
        }
    }
    //_Distort_Speed  //GlowPower   //_Alpha
    private void StartChoose()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraRotate.cameratotate = false;
        Character.AllProhibit = true;
        Character.ActionProhibit = true;
        buttonCreator.dialogAssets = chooseDialog.dialogAsset;
        buttonCreator.option = chooseDialog.ChooseDialog;
        buttonCreator.CreateButtonList();
        OnChoosing = true;
        Choose = false;
    }
    private void ShaderToTarget()
    {
        var colorModule = particle.colorOverLifetime;
        colorModule.color = new ParticleSystem.MinMaxGradient(new Color(1,1,1,1));
        textComponent.color = default_color;
        target_material.SetFloat("_Distort_Speed", DistortSpeed[index]);
        target_material.SetColor("_Color", color[index]);
        target_material.SetFloat("_GlowPower", ShaderGlow[index]);
        target_material.SetFloat("_Alpha", Alpha[index]);
        textComponent.color = color[index];

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
    private IEnumerator FpsFadein(float speed, int fps_rate)
    {
        var Module = particle.textureSheetAnimation;
        float target_fps = 8 + Mathf.Abs(DistortSpeed[index]) * fps_rate;
        float difference = target_fps - 8;
        for (float i = 0; i < 1; i += 1 / target_fps * speed)
        {
            Module.fps = 8 + difference * i;
            yield return new WaitForSeconds(1 / target_fps);
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
        for (float i = 0; i < 1; i += Time.deltaTime*2)
        {
            Color target_color = start_ + difference * i;
            //Color target_color2 = target_color;
            Color target_color2 = new Color(1,1, 1, 0.95f);
            var colorModule = particle.colorOverLifetime;
            if (index == 0)
                target_color2.a = i;
            else
            {
                target_color2.a = (i + 1) / 2f;
                if (colorModule.color.color.a > (i + 1f) / 2f)
                    target_color2.a = colorModule.color.color.a;
            }
            target_material.SetColor("_Color", target_color);
            colorModule.color = new ParticleSystem.MinMaxGradient(target_color2);
            textComponent.color = target_color;
            yield return null;
        }
    }
    private IEnumerator ColorFadein2(Color target,float speed)
    {
        Color start_ = target_material.GetColor("_Color");
        Color difference = (target - start_);
        for (float i = 0; i < 1; i += Time.deltaTime *speed)
        {
            Color target_color = start_ + difference * i;
            Color target_color2 = target_color;
            target_material.SetColor("_Color", target_color);
            textComponent.color = target_color;
            yield return null;
        }
    }
    private IEnumerator DialogFrameFadeout(float target)
    {
        Color start_ = target_material.GetColor("_Color");
        Color target_color = target_material.GetColor("_Color");
        Color target_color2 = target_color;
        target_color2 = (target_color + Color.white * 0.4f) * 0.6f;
        target_color = (target_color + Color.white * 0.4f) * 0.1f;
        Color difference = (target_color - start_);
        float start_alpha = target_material.GetFloat("_Alpha");
        StartCoroutine(ColorFadein2(target_color2, 0.8f)); ;
        for (float i = start_alpha; i > target; i -= Time.deltaTime*0.6f)
        {
            target_color = start_ + difference * (start_alpha-i);
            target_color.a = i;
            var colorModule = particle.colorOverLifetime;
            colorModule.color = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, i));
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
