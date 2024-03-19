using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light flashlightLight;
    public float maxIntensity = 1f;
    public float scrollSpeed = 0.5f;
    public float flickerTime = 0.2f;
    public int flashtimes = 1;
    public AudioSource source;
    public AudioClip[] clip;
    private bool isFlashlightOn = false;
    private float originalIntensity;

    void Start()
    {
        originalIntensity = flashlightLight.intensity;
    }

    public Transform PlayerCamera;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && UsePeep.DeviceUse == false)
        {
            if (!isFlashlightOn)
            {
                source.PlayOneShot(clip[0]);
                StartCoroutine(TurnOnFlashlight(flashtimes));
            }
            else
            {
                source.PlayOneShot(clip[1]);
                TurnOffFlashlight();
            }       
        }
        Vector3 TargetRotation = new Vector3(PlayerCamera.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 OriginRotation = transform.eulerAngles;
        if (TargetRotation.x > 180)
        {
            TargetRotation.x -= 360;
        }
        if (OriginRotation.x > 180)
        {
            OriginRotation.x -= 360;
        }
        transform.eulerAngles = Vector3.Lerp(OriginRotation, TargetRotation, 0.2f);
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
            Scroll();
    }

    private float lightPercent = 0.7f;
    private void Scroll()
    {
        lightPercent += Input.mouseScrollDelta.y * scrollSpeed;
        lightPercent = Mathf.Clamp(lightPercent, 0.13f, 1);
        flashlightLight.intensity = maxIntensity * lightPercent;
    }
    IEnumerator TurnOnFlashlight(int times)
    {
        isFlashlightOn = true;
        flashlightLight.enabled = true;
        flashlightLight.intensity = maxIntensity* lightPercent;
        for(int i =0; i < flashtimes; i++)
        {
            yield return new WaitForSeconds(Random.Range(flickerTime - 0.01f, flickerTime + 0.01f));
            flashlightLight.intensity = 0.2f* lightPercent;
            yield return new WaitForSeconds(Random.Range(flickerTime - 0.01f, flickerTime + 0.01f));
            flashlightLight.intensity = maxIntensity* lightPercent;
        }
             
        flashlightLight.intensity = maxIntensity* lightPercent;
    }

    public void TurnOffFlashlight()
    {
        isFlashlightOn = false;
        flashlightLight.enabled = false;
        flashlightLight.intensity = originalIntensity* lightPercent;
    }
    public IEnumerator LightWeak()
    {
        if (!isFlashlightOn)
            yield break;
        source.clip = clip[2];
        Debug.Log(Character.NoEnergy);
        source.PlayOneShot(clip[2]);
        flashlightLight.intensity = maxIntensity* lightPercent;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(Random.Range(flickerTime - 0.01f, flickerTime + 0.01f));
            flashlightLight.intensity = 0.1f* lightPercent;
            yield return new WaitForSeconds(Random.Range(flickerTime - 0.01f, flickerTime + 0.01f));
            flashlightLight.intensity = maxIntensity / 2* lightPercent;
        }

        flashlightLight.intensity = maxIntensity/3* lightPercent;
        yield return new WaitForSeconds(1f);
        flashlightLight.enabled = false;
    }
}
