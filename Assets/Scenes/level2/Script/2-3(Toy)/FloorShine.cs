using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorShine : MonoBehaviour
{
    FloorControll floorControll;
    public FloorControll.FloorColor floorColor;
    public float blinkSpeed = 2.0f; 
    private Color originalColor;
    public Color targetColor = new Color(0.17f, 0.05f, 0.05f);
    private Renderer render;
    void Start()
    {
        floorControll = FloorControll.Instance;
        render = GetComponent<Renderer>();
        if (render != null)
        {
            originalColor = render.material.GetColor("_EmissionColor"); // "_EmissionColor" ê•ópò“çTêßé©·¢åıìI Shader ô“ù…ñº‚i
            //render.material.SetColor("_EmissionColor", new Color(0.17f, 0.05f, 0.05f, 0));
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
    }

    void Update()
    {
        if (floorControll.floor == floorColor)
        {
            Shine();
        }
        else
        {
            render.material.SetColor("_EmissionColor", originalColor);
        }
    }
    private void Shine()
    {
        float emission = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);

        Color newColor = originalColor + targetColor * emission;

        render.material.SetColor("_EmissionColor", newColor);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Run")
        {
            floorControll.floor = floorColor;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Run")
        {
            floorControll.floor = FloorControll.FloorColor.white;
        }
    }
}
