using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOff : MonoBehaviour
{
    private Light SpotLight;
    private Transform Character;

    private float initialIntensity;
    public float OffDistance = 40;
    public float DecreasingRate = 1f;
    public bool Off_On = true;
    private void Start()
    {
        SpotLight = this.GetComponent<Light>();
        Character = GameObject.Find("Character").transform;
        initialIntensity = SpotLight.intensity;
    }


    void Update()
    {
        float intensityDelta = Vector3.Distance(Character.transform.position, transform.position);
        if(Off_On == true)
        {
            if (intensityDelta > OffDistance)
            {
                SpotLight.intensity = Mathf.Max(initialIntensity - (intensityDelta - OffDistance) * DecreasingRate, 0.0f);
                if (SpotLight != null && intensityDelta > OffDistance + initialIntensity)
                {
                    SpotLight.enabled = false;
                }
            }
            else
            {
                initialIntensity = SpotLight.intensity;
                if (SpotLight != null)
                {
                    if (SpotLight.enabled == false)
                    {
                        SpotLight.enabled = true;
                        SpotLight.intensity = initialIntensity;
                    }
                }
            }
        }

    }
}
