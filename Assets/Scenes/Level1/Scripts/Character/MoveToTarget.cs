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
    private Vector3 OriginPos;
    public float movementSpeed = 5f;

    private CharacterController controller;

    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private void Start()
    {
        OriginPos = LookPoint.localPosition;
        controller = character.GetComponent<CharacterController>();
        previousPosition = character.transform.position+Vector3.forward;
        currentPosition = character.transform.position;
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
        c += Time.deltaTime;
        currentPosition = character.transform.position;
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
            direction = target.position - character.transform.position;
            if (new Vector2(direction.x,direction.z).magnitude> 0.22f)
            {
                direction.Normalize();
                direction.y = controller.velocity.y/ movementSpeed;
                controller.Move(direction * movementSpeed * Time.deltaTime);              
            }
            else if(character.transform.position.y>-1f)
            {
                finish = true;
                StartCoroutine(RotateToStage());

            }

        }
        previousPosition = currentPosition;
    }
    private IEnumerator RotateToStage()
    {

        Vector3 targetRotation = new Vector3(character.transform.eulerAngles.x, 0, character.transform.eulerAngles.z);
        Vector3 CurrentEulerAngles = character.transform.eulerAngles;
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
            character.transform.eulerAngles = CurrentEulerAngles;
            if(Mathf.Abs(CurrentEulerAngles.y - targetRotation.y) < 1)
            {
                StartCoroutine(StageLightSet());
                StartCoroutine(CameraRoutine());
                CameraRotate.cameratotate = true;
                yield break;
            }
            yield return null;
        }
    }

    public AudioCharacter characteraudio;
    public AudioSource source;
    public Animator Anim_Move;
    public GameObject AllLight;
    public GameObject DynamicLight1;
    public GameObject DynamicLight2;
    public GameObject Light_Boss_S;
    public GameObject Light_Boss_P;
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
        while (elapsedTime < 3.5f)
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


        elapsedTime = 0f;
        while (elapsedTime < 3.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 3.5f;
            light1.intensity = Mathf.Lerp(0,4, t);
            light2.intensity = Mathf.Lerp(0,4, t);
            light6.intensity = Mathf.Lerp(0, 2f, t);
            light7.intensity = Mathf.Lerp(0, 2f, t);

            colorAdjustments.postExposure.value = Mathf.Lerp(0, initialExposure-1.5f, t);
            if (elapsedTime > 1&& elapsedTime<1.1f)
            {
                Curtain1.enabled = true;
                Curtain2.enabled = true;
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
            light1.intensity = Mathf.Lerp(4, 8, t);
            light2.intensity = Mathf.Lerp(4, 8, t);
            light6.intensity = Mathf.Lerp(1, 6f, t);
            light7.intensity = Mathf.Lerp(1, 8f, t);
            yield return null;
        }
        FakeSpotlight1.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        FakeSpotlight2.SetActive(true);
        yield return new WaitForSeconds(11f);

        Color Origin1 = light1.color;
        Color Origin2 = light1.color;
        Color Origin6 = light6.color;
        Color Origin7 = light7.color;
        elapsedTime = 0f;
        while (elapsedTime < 11f)
        {   
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 10;
            light1.color = Color.Lerp(Origin1, new Color(1f, 0.25f, 0.25f, 1f), t);
            light2.color = Color.Lerp(Origin2, new Color(1f, 0.25f, 0.25f, 1f), t);
            light6.color = Color.Lerp(Origin6, new Color(1f, 0.25f, 0.25f, 1f), t);
            light7.color = Color.Lerp(Origin7, new Color(1f, 0.25f, 0.25f, 1f), t);
            yield return null;
        }
        AllLight.SetActive(false);
        DynamicLight1.SetActive(false);
        DynamicLight2.SetActive(false);
        Light_Boss_S.SetActive(false);
        source.PlayOneShot(characteraudio.AudioClip[6]);
        //source.PlayOneShot(audio.AudioClip[7]);
        yield return new WaitForSeconds(1.5f);
        DynamicLight1.SetActive(true);
        Light_Boss_S.SetActive(true);
        panic.State = 0;
        yield return new WaitForSeconds(0.5f);
        panic.State = 0;
        yield return new WaitForSeconds(2.6f);
        Light_Boss_S.SetActive(false);
        DynamicLight1.SetActive(false);
        yield return new WaitForSeconds(1.7f);
        Anim_Move.SetTrigger("Action2");
        LookPoint.localPosition = OriginPos;
        yield return new WaitForSeconds(0.5f);
        panic.State = 0;
        AllLight.SetActive(true);
        DynamicLight1.SetActive(true);
        DynamicLight2.SetActive(true);
        Light_Boss_S.SetActive(true);
        Light_Boss_P.SetActive(true);
        yield return new WaitForSeconds(1f);
        Character.AllProhibit = false;
        Character.ActionProhibit = false;
        CameraRotate.cameratotate = true;
        yield return new WaitForSeconds(5);
        this.enabled = false;

    }

    public PanicRed_PP panic;
    private IEnumerator CameraRoutine()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(CameraMoveUpOrDown(1,1));
        yield return new WaitForSeconds(34);

    }
    private IEnumerator CameraMoveUpOrDown(int UpDown,int FrontBack)
    {
        Vector3 TargetPos = LookPoint.position + Vector3.up * UpDown + Vector3.left* FrontBack;
        while (true)
        {
            Vector3 LookPointMove = Vector3.Lerp(LookPoint.position, TargetPos, 0.5f);
            LookPoint.position = LookPointMove;
            if (Vector3.Distance(LookPoint.position, TargetPos) < 0.03f)
            {
                LookPoint.position = TargetPos;
                yield break;
            }
            yield return null;
        }

    }

}
