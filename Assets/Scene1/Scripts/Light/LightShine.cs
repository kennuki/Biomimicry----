using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShine : MonoBehaviour
{
    public Light Spotlight;
    public Light Pointlight;
    public Renderer Light;

    void Start()
    {
        Spotlight_Origin_Intensity = Spotlight.intensity;
        Pointlight_Origin_Intensity = Pointlight.intensity;
        Light_Origin_Emission = Light.material.GetVector("_EmissionColor");
    }

    float Counter = 0;
    float RandomTime =0.5f;
    float Spotlight_Origin_Intensity;
    float Pointlight_Origin_Intensity;
    Vector3 Light_Origin_Emission;
    bool FadeAllow = false;
    private float intensity = 1;
    void Update()
    {
        Counter += Time.deltaTime;
        if (Counter > RandomTime)
        {
            if (RandomTime < 0.2f)
            {
                RandomTime = Random.Range(0.01f, 0.24f);
            }
            else
            {
                RandomTime = Random.Range(0.01f, 1f);

            }
            if (Spotlight.intensity == 0.5f)
            {
                intensity = 1;
                Spotlight.intensity = Spotlight_Origin_Intensity * Mathf.Clamp((RandomTime + 0.4f),0,1f)  / 1f;
                Pointlight.intensity = Pointlight_Origin_Intensity * Mathf.Clamp((RandomTime + 0.4f), 0, 1f) / 1f;
                Light.material.SetVector("_EmissionColor", Light_Origin_Emission * intensity);
                FadeAllow = false;
                if (RandomTime > 0.4f)
                {
                    RandomTime *= 2.2f;
                }

            }
            else
            {

                Spotlight.intensity = 0.5f;
                Pointlight.intensity = 0f;
                intensity = 0.4f;
                Light.material.SetVector("_EmissionColor", Light_Origin_Emission * intensity);
                FadeAllow = true;
            }
            Counter = 0;

        }
        if (FadeAllow == true)
        {
            intensity -= Time.deltaTime*1.5f;
            Light.material.SetVector("_EmissionColor", Light_Origin_Emission * (intensity));
        }

    }
}
