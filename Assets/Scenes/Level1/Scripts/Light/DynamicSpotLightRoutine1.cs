using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpotLightRoutine1 : MonoBehaviour
{
    public Light targetLight;
    public Material fakeLightMaterial;
    public float rotationDuration = 5f;
    public float xRotationDuration = 1f;
    public float maxRotationAngle = 160f;
    public float maxRotationAngleX = -30f;
    public float maxLightIntensity = 10f;
    public float minLightIntensity = 0.5f;
    public float lightFlickerRate = 1f;
    public float fakeLightFalloffValue = 0.8f;
    public float alphaChangeRate = 0.02f;

    private float initialLightIntensity;
    public Renderer fakeLightRenderer;
    private float initialFakeLightAlpha;
    private float timer;

    private void Start()
    {
        StartCoroutine(RotateObject());
        StartCoroutine(ChangeLightIntensity());
        StartCoroutine(ChangeFakeLightProperties());
    }

    private IEnumerator RotateObject()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, maxRotationAngle, 0f)* transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / Mathf.Lerp(rotationDuration*3, rotationDuration, elapsedTime);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;
        startRotation = transform.localRotation;
        Quaternion xTargetRotation = Quaternion.Euler(maxRotationAngleX, 0f, 0f)* startRotation;
        while (elapsedTime < xRotationDuration)
        {
            float t = elapsedTime / Mathf.Lerp(xRotationDuration/2, xRotationDuration, elapsedTime);
            transform.localRotation = Quaternion.Slerp(startRotation, xTargetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //transform.localRotation = xTargetRotation;
        //transform.rotation = targetRotation;
    }

    private IEnumerator ChangeLightIntensity()
    {
        initialLightIntensity = targetLight.intensity;

        // Increase light intensity to max
        targetLight.intensity = maxLightIntensity;
        yield return new WaitForSeconds(1f);

        // Light flickering
        timer = 0f;
        while (timer < 4f)
        {
            targetLight.intensity = Mathf.Lerp(maxLightIntensity, minLightIntensity, Mathf.PingPong(timer * lightFlickerRate, 1f));
            timer += Time.deltaTime;
            yield return null;
        }

        // Set light intensity to max
        targetLight.intensity = maxLightIntensity;
    }

    private IEnumerator ChangeFakeLightProperties()
    {
        initialFakeLightAlpha = fakeLightRenderer.material.GetFloat("_Opacity");

        // Change fake light alpha
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            float t = elapsedTime / 1f;
            float a = Mathf.Lerp(initialFakeLightAlpha, maxLightIntensity* alphaChangeRate, t);
            float falloff = Mathf.Lerp(0, fakeLightFalloffValue, t);
            fakeLightRenderer.material.SetFloat("_Opacity", a);
            fakeLightRenderer.material.SetFloat("_EdgeFallof", falloff);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(23);
        Color Origin1 = fakeLightRenderer.material.GetColor("_Color");
        Color Origin2 = targetLight.color;
        elapsedTime = 0f;
        while (elapsedTime < 7f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 7;
            fakeLightRenderer.material.SetColor("_Color", Color.Lerp(Origin1, new Color(1f, 0.25f, 0.25f, 1f), t));
            targetLight.color = Color.Lerp(Origin2, new Color(1f, 0.25f, 0.25f, 1f), t);
            yield return null;
        }
    }
}

