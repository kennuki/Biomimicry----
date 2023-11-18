using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorShine : MonoBehaviour
{
    public float blinkSpeed = 2.0f; 
    private Color originalColor;
    private Color targetColor = new Color(0.17f, 0.05f, 0.05f);
    private Renderer render;
    void Start()
    {
        render = GetComponent<Renderer>();
        if (render != null)
        {
            originalColor = render.material.GetColor("_EmissionColor"); // "_EmissionColor" ê•ópò“çTêßé©·¢åıìI Shader ô“ù…ñº‚i
            Debug.Log(originalColor);
            //render.material.SetColor("_EmissionColor", new Color(0.17f, 0.05f, 0.05f, 0));
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
    }

    void Update()
    {
        
    }
    private void Shine()
    {
        float emission = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);

        Color newColor = originalColor + targetColor * emission;

        render.material.SetColor("_EmissionColor", newColor);
    }
}
