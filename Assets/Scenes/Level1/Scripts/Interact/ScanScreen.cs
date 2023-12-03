using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanScreen : MonoBehaviour
{
    public AudioSource source;
    public AudioClip BB;
    public AudioClip DoorOpen;
    public Animator GateOpen;
    public Rigidbody[] DoorRb;
    public static bool ScanResult = false;
    public static bool Scanning = false;
    private float ScanTime = 1.5f;
    private float FailShowTime = 3f;
    float counter_Scan = 0,counter_FailShow = 0; 
    private Renderer render;
    public Texture[] image;
    public Renderer MojiRender;
    private void Start()
    {
        render = GetComponent<Renderer>();
        currentIntensity = MojiRender.material.GetColor("_EmissionColor").maxColorComponent;
    }

    bool Success = false;
    private void Update()
    {
        if(Scanning == true && Success ==false)
        {
            counter_Scan += Time.deltaTime;
            if (counter_Scan > ScanTime)
            {
                if(ScanResult == false)
                {
                    counter_FailShow += Time.deltaTime;
                    if (counter_FailShow > FailShowTime)
                    {
                        render.material.SetTexture("_BaseMap", image[0]);
                        render.material.SetTexture("_EmissionMap", image[0]);
                        MojiRender.transform.gameObject.SetActive(false);
                        counter_FailShow = 0;
                        counter_Scan = 0;
                        Scanning = false;
                    }
                    else if (counter_FailShow <= 0.5f)
                    {
                        render.material.SetTexture("_BaseMap", image[1]);
                        render.material.SetTexture("_EmissionMap", image[1]);
                        MojiRender.transform.gameObject.SetActive(true);
                        MojiRender.material.SetTexture("_BaseMap", image[3]);
                        MojiRender.material.SetTexture("_EmissionMap", image[3]);
                    }
                }
                else if(ScanResult == true)
                {
                    render.material.SetTexture("_BaseMap", image[2]);
                    render.material.SetTexture("_EmissionMap", image[2]);
                    MojiRender.transform.gameObject.SetActive(true);
                    MojiRender.material.SetTexture("_BaseMap", image[4]);
                    MojiRender.material.SetTexture("_EmissionMap", image[4]);
                    GateOpen.enabled = true;
                    foreach(Rigidbody rb in DoorRb)
                    {
                        rb.isKinematic = false;
                    }
                    Success = true;
                    source.volume = 0.3f;
                    source.clip = DoorOpen;
                    source.Play();
                }
            }
            if (counter_Scan < 1.2f && counter_Scan > 1.1f)
            {
                source.volume = 0.1f;
                source.clip = BB;
                source.Play();
            }
        }
        FlickFunction();
    }

    public float minIntensity = 0.6f;
    public float maxIntensity = 1.0f;
    public float flickerSpeed = 2.0f;

    private float currentIntensity;
    private float targetIntensity;
    private float timer;
    private void FlickFunction()
    {
        timer += Time.deltaTime;


        if (timer >= 1.0f / flickerSpeed)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timer = 0.0f;
        }

        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, timer * flickerSpeed);

        Color newEmissionColor = new Color(currentIntensity, currentIntensity, currentIntensity);
        MojiRender.material.SetColor("_EmissionColor", newEmissionColor);

        MojiRender.material.EnableKeyword("_EMISSION");
    }


}
