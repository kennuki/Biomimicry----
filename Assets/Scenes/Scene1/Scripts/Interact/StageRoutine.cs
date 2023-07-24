using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRoutine : MonoBehaviour
{
    public Animator Switch;

    public GameObject[] objectsToEnable;
    public Material emissionMaterial;
    public float targetEmission = 1f;
    public float flashProbability = 0.2f;
    public float flashDuration = 0.5f;
    public float flashTime = 0.1f;
    public float minDelay = 0f;
    public float maxDelay = 0.1f;
    private int currentIndex = 0;


    public Light targetLight;
    public Light targetLight2;
    public float targetIntensity; 
    public float fadeDuration = 1.5f;

    public Light targetLight3;
    public Light targetLight4;
    public float targetIntensity2;
    public float fadeDuration2 = 1.5f;



    private float startIntensity; 
    private float timer; 
    private void Start()
    {
        Switch.enabled = true;
        startIntensity = targetLight.intensity; 
        timer = 0f; 
        StartCoroutine(EnableObjectsWithDelay());
    }

    private System.Collections.IEnumerator EnableObjectsWithDelay()
    {
        yield return new WaitForSeconds(4f);
        while (currentIndex < objectsToEnable.Length)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);


            Renderer renderer = objectsToEnable[currentIndex].GetComponent<Renderer>();

            Material material = new Material(emissionMaterial);
            renderer.material = material;

            StartCoroutine(AnimateEmission(material));



            currentIndex++;
            yield return null;
        }
    }

    private System.Collections.IEnumerator AnimateEmission(Material material)
    {
        Color targetEmissionColor = Color.white * targetEmission;
        if(currentIndex == 0)
        {
            targetEmissionColor = Color.white * 4;
        }
        float elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;
            Color emissionColor = Color.Lerp(Color.black, targetEmissionColor, elapsedTime);
            material.SetColor("_EmissionColor", emissionColor);
            yield return null;
        }
        material.SetColor("_EmissionColor", targetEmissionColor);
        if (Random.value < flashProbability)
        {
            yield return StartCoroutine(AnimateFlashing(material));
        }
    }

    private System.Collections.IEnumerator AnimateFlashing(Material material)
    {
        Color initialEmissionColor = material.GetColor("_EmissionColor");
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
            material.SetColor("_EmissionColor", newEmissionColor);
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
        material.SetColor("_EmissionColor", initialEmissionColor);
        this.enabled = false;
    }

    private void Update()
    {

        timer += Time.deltaTime;
        if (timer > 4)
        {
            if (timer <= fadeDuration+4)
            {
                float t = timer-4 / fadeDuration;
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
                float t2 = timer-4 / fadeDuration2;
                float currentIntensity2 = Mathf.Lerp(0, targetIntensity2, t2);
                targetLight3.intensity = currentIntensity2;
                targetLight4.intensity = currentIntensity2;
            }
            else
            {
                targetLight3.intensity = targetIntensity2;
                targetLight4.intensity = targetIntensity2;
            }
        }
       
    }
}
