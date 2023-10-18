using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRoutine : MonoBehaviour
{
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToEnable2;
    public float[] intensity;
    public float targetEmission = 1f;
    public float flashProbability = 0.2f;
    public float flashDuration = 0.5f;
    public float flashTime = 0.1f;
    public float minDelay = 0f;
    public float maxDelay = 0.1f;


    public Light targetLight;
    public Light targetLight2;
    public float targetIntensity; 
    public float fadeDuration = 1.5f;

    public Light targetLight3;
    public Light targetLight4;
    public float targetIntensity2;
    public float fadeDuration2 = 1.5f;

    public Light targetLight5;
    public Light targetLight6;
    public Light targetLight7;
    public float targetIntensity3;
    public float targetIntensity4;

    private float startIntensity; 
    private float timer;

    public GameObject GreenLight_Dark, GreenLight, BlueLight_Dark, BlueLight;
    public GameObject EmergencyLight, EmergencyLight_Dark;
    private void Start()
    {
        startIntensity = targetLight.intensity; 
        timer = 0f; 
        StartCoroutine(EnableObjectsWithDelay(objectsToEnable, intensity[0]));
        StartCoroutine(EnableObjectsWithDelay(objectsToEnable2, intensity[1]));
        StartCoroutine(DynamoLightSet());
    }

    private System.Collections.IEnumerator EnableObjectsWithDelay(GameObject[]gameObjects, float Intensity)
    {
        int currentIndex = 0;
        yield return new WaitForSeconds(4f);
        while (currentIndex < gameObjects.Length)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);



            Renderer renderer = gameObjects[currentIndex].GetComponent<Renderer>();


            StartCoroutine(AnimateEmission(renderer, Intensity));



            currentIndex++;
            yield return null;
        }
    }

    private System.Collections.IEnumerator AnimateEmission(Renderer render,float Intensity)
    {
        Color targetEmissionColor = render.material.GetColor("_BaseColor") * Intensity;
        yield return null;
        /*if(currentIndex == 0)
        {
            targetEmissionColor = Color.white * Intensity;
        }*/
        float elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;
            Color emissionColor = Color.Lerp(Color.black, targetEmissionColor, elapsedTime);
            render.material.SetColor("_EmissionColor", targetEmissionColor);
            yield return null;
        }
        render.material.SetColor("_EmissionColor", targetEmissionColor);
        if (Random.value < flashProbability)
        {
            yield return StartCoroutine(AnimateFlashing(render));
        }
    }

    private System.Collections.IEnumerator AnimateFlashing(Renderer render)
    {
        Color initialEmissionColor = render.material.GetColor("_EmissionColor");
        float minEmission = targetEmission / 4f + Random.Range(0f, targetEmission / 2f);
        float maxEmission = targetEmission;
        float counter =0f;
        float elapsedTime = 0f;
        flashDuration = Random.Range(flashDuration - 0.1f, flashDuration + 0.1f);
        bool increasing = true;

        while (elapsedTime < flashDuration)
        {
            flashTime = Random.Range(flashTime - 0.05f, flashTime + 0.05f);
            float t = counter / flashTime;
            float emission = increasing ? Mathf.Lerp(minEmission, maxEmission, t) : Mathf.Lerp(maxEmission, minEmission, t);
            Color newEmissionColor = Color.white * emission;
            render.material.SetColor("_EmissionColor", newEmissionColor);
            if (t >= 1f)
            {
                counter = 0f;
                increasing = !increasing;
                minEmission = targetEmission / 4f + Random.Range(0f, targetEmission / 2f);
            }
            counter += Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        render.material.SetColor("_EmissionColor", initialEmissionColor);
        this.enabled = false;
    }

    private IEnumerator DynamoLightSet()
    {
        yield return new WaitForSeconds(1);
        GreenLight_Dark.SetActive(false);
        GreenLight.SetActive(true);
        BlueLight_Dark.SetActive(true);
        BlueLight.SetActive(false);
        EmergencyLight.SetActive(true);
        EmergencyLight_Dark.SetActive(false);
        yield break;
    }

    private void Update()
    {

        timer += Time.deltaTime;
        if (timer > 4)
        {
            if (timer <= fadeDuration+4)
            {
                float t = (timer-4) / fadeDuration;
                float currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, t);
                targetLight.intensity = currentIntensity;
                targetLight2.intensity = currentIntensity;
            }
            else
            {
                targetLight.intensity = targetIntensity;
                targetLight2.intensity = targetIntensity;
            }
            if (timer <= fadeDuration2+4)
            {
                float t = (timer - 4) / fadeDuration2;
                float currentIntensity = Mathf.Lerp(0, targetIntensity2, t);
                targetLight3.intensity = currentIntensity;
                targetLight4.intensity = currentIntensity;
            }
            else
            {
                targetLight3.intensity = targetIntensity2;
                targetLight4.intensity = targetIntensity2;
            }
            if (timer <= fadeDuration2 + 4)
            {
                float t = (timer - 4) / fadeDuration2;
                float currentIntensity = Mathf.Lerp(0, targetIntensity3, t);
                targetLight5.intensity = currentIntensity;
                targetLight6.intensity = currentIntensity;
            }
            else
            {
                targetLight5.intensity = targetIntensity3;
                targetLight6.intensity = targetIntensity3;
            }
            if (timer <= fadeDuration2 + 4)
            {
                float t = (timer - 4) / fadeDuration2;
                float currentIntensity = Mathf.Lerp(0, targetIntensity4, t);
                targetLight7.intensity = currentIntensity;
            }
            else
            {
                targetLight7.intensity = targetIntensity4;
            }
        }
       
    }
}
