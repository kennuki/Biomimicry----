using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MoveToTarget : MonoBehaviour
{
    public DynamicSpotLightRoutine1[] dynamicSpotLights;
    public Animator Curtain1;
    public Animator Curtain2;
    public Transform LookPoint;
    public Character character;
    public Transform target;
    public float movementSpeed = 5f;

    private CharacterController controller;

    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        previousPosition = transform.position+Vector3.forward;
        currentPosition = transform.position;
        postProcessProfile = postProcessVolume.profile;
        clonedProfile = Instantiate(postProcessProfile);
        if (clonedProfile.TryGet(out colorAdjustments))
        {
            initialExposure = colorAdjustments.postExposure.value;
        }
    }

   

    Vector3 direction;
    float c = 0;
    bool finish = false;
    private void FixedUpdate()
    {
;
        c += Time.deltaTime;
        currentPosition = transform.position;
        if (Vector3.Distance(currentPosition, previousPosition) < 0.02f  && finish == false && c > 1)
        {
            c = 0;
            character.Jump(0.8f);

        }
        if (finish==false)
        {
            CameraRotate.cameratotate = false;
            Character.AllProhibit = true;
            Character.MoveOnly = false;
            direction = target.position - transform.position;
            if (direction.magnitude > 0.62f)
            {
                direction.Normalize();
                direction.y = controller.velocity.y/ movementSpeed;
                controller.Move(direction * movementSpeed * Time.deltaTime);              
            }
            else if(transform.position.y>-1)
            {
                finish = true;
                StartCoroutine(RotateToStage());
                
            }

        }
        previousPosition = currentPosition;
        if(finish == true)
        {
            Character.AllProhibit = true;
            Character.MoveOnly = false;
        }
    }
    private IEnumerator RotateToStage()
    {
        Vector3 targetRotation = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        Vector3 CurrentEulerAngles = transform.eulerAngles;
        while (true)
        {
            if (CurrentEulerAngles.y > 180)
            {
                //CurrentEulerAngles-= new Vector3(0, 360, 0);
            }
            if (CurrentEulerAngles.y - targetRotation.y > 180)
            {
                CurrentEulerAngles -= new Vector3(0, 360, 0);
            }
            CurrentEulerAngles = Vector3.Lerp(CurrentEulerAngles, targetRotation, 0.02f);
            transform.eulerAngles = CurrentEulerAngles;
            if(Mathf.Abs(CurrentEulerAngles.y - targetRotation.y) < 1)
            {
                StartCoroutine(StageLightSet());
                CameraRotate.cameratotate = true;
                yield break;
            }
            yield return null;
        }

    }

    public GameObject Boss;
    public Light light1;
    public Light light2;
    public Light light3;
    public Light light4;
    public Light light5;
    public Light light6;
    public Light light7;
    public Light light8;
    public Light light9;
    public GameObject FakeSpotlight1;
    public GameObject FakeSpotlight2;
    public Renderer[] StageLight;
    public Volume postProcessVolume;
    private VolumeProfile postProcessProfile;
    private VolumeProfile clonedProfile;
    private ColorAdjustments colorAdjustments;
    private float initialExposure;
    private IEnumerator StageLightSet()
    {
        float elapsedTime = 0f;
        float OriginIntensity1 = light1.intensity;
        float OriginIntensity2 = light2.intensity;
        float OriginIntensity3 = light3.intensity;
        float OriginIntensity4 = light4.intensity;
        float OriginIntensity5 = light5.intensity;
        float OriginIntensity8 = light8.intensity;
        float OriginIntensity9 = light9.intensity;
        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 3;
            foreach (Renderer renderer in StageLight)
            {
                Color emissionColor = Color.Lerp(renderer.material.GetColor("_EmissionColor"), Color.black, t);
                renderer.material.SetColor("_EmissionColor", emissionColor);
            }
            light1.intensity = Mathf.Lerp(OriginIntensity1, 0, t);
            light2.intensity = Mathf.Lerp(OriginIntensity2, 0, t);
            light3.intensity = Mathf.Lerp(OriginIntensity3, 0, t);
            light4.intensity = Mathf.Lerp(OriginIntensity4, 0, t);
            light5.intensity = Mathf.Lerp(OriginIntensity5, 0, t);
            light8.intensity = Mathf.Lerp(OriginIntensity8, 0, t);
            light9.intensity = Mathf.Lerp(OriginIntensity9, 0, t);
            colorAdjustments.postExposure.value = Mathf.Lerp(initialExposure, 0, t);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        Curtain1.enabled = true;
        Curtain2.enabled = true;
        elapsedTime = 0f;
        while (elapsedTime < 3.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 3.5f;
            light1.intensity = Mathf.Lerp(0,4, t);
            light2.intensity = Mathf.Lerp(0,4, t);
            light6.intensity = Mathf.Lerp(0, 1f, t);
            light7.intensity = Mathf.Lerp(0, 1f, t);

            colorAdjustments.postExposure.value = Mathf.Lerp(0, initialExposure-1.5f, t);
            if (elapsedTime > 1&& elapsedTime<1.1f)
            {
                foreach (DynamicSpotLightRoutine1 routine1 in dynamicSpotLights)
                {
                    routine1.enabled = true;
                }
            }
            
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < 4f)
        {
            if (elapsedTime > 0.5f)
            {
                Boss.SetActive(true);
            }
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 3;
            light6.intensity = Mathf.Lerp(1, 20f, t);
            light7.intensity = Mathf.Lerp(1, 20f, t);
            yield return null;
        }
        FakeSpotlight1.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        FakeSpotlight2.SetActive(true);
        this.enabled = false;

    }

}
