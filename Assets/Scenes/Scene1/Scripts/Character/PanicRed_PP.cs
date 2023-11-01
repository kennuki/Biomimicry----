using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PanicRed_PP : MonoBehaviour
{
    private AudioSource CharacterAudioSourse;
    private AudioSource CharacterAudioSourseLoop;
    private AudioCharacter AudioCharacter;

    private GameObject NoiseAffect;
    private Renderer render;

    public Volume postProcessVolume;
    public VolumeProfile postProcessProfile;
    public float vignetteSpeed = 1f;
    public float saturationSpeed = 1f;
    public float vignetteAmplitude = 0.5f;
    public float saturationAmplitude = 0.5f;

    public int State = 3;
    public float RunTime = 2f;
    public bool quick = false;
    private float counter = 0;

    private Vignette vignette;

    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        CharacterAudioSourse = GameObject.Find("Character").transform.Find("PlayerAudio").GetComponent<AudioSource>();
        AudioCharacter = CharacterAudioSourse.gameObject.GetComponent<AudioCharacter>();
        CharacterAudioSourseLoop = AudioCharacter.AudioSources[1];
        OriginVolume = CharacterAudioSourse.volume;
        OriginVolumeLoop = CharacterAudioSourseLoop.volume;

        NoiseAffect = Camera.main.gameObject.transform.Find("Plane").gameObject;
        render = NoiseAffect.GetComponent<Renderer>();

        postProcessVolume = gameObject.GetComponent<Volume>();
        postProcessVolume.isGlobal = true;

        
        postProcessVolume.sharedProfile = postProcessProfile;

        
        postProcessProfile.TryGet(out vignette);
        vignette.active = false;
    }

    
    void Update()
    {
        if(CharacterAudioSourse == null)
        {
            FindAudioSourse();
        }
        NoiseFunction();
        if (Input.GetKeyDown(KeyCode.N))
        {
            State = 0;
        }

    }


    float NoiseTime = 0;
    float vignetteIntensity = 0;
    float saturationValue = 0;

    float NoiseDelayCount = 0;
    float OriginVolume;
    float OriginVolumeLoop;

    public float NoiseMaxStrenth = 0.4f; 
    private void NoiseFunction()
    {
        if (counter > RunTime)
        {
            State = 2;
            counter = 0;
        }
        if(State == 0)
        {
            vignette.active = true;
            if (quick)
            {
                counter += Time.deltaTime*1;
            }
            else
            {
                counter += Time.deltaTime;
            }
            NoiseAffect.SetActive(true);
            if (render.material.GetFloat("_Strength") < NoiseMaxStrenth)
            {
                render.material.SetFloat("_Strength", render.material.GetFloat("_Strength") + Time.deltaTime * 0.3f);
            }


            if (CharacterAudioSourse.volume <= OriginVolume)
            {
                CharacterAudioSourse.volume = counter * 0.2f;
            }
            if (!CharacterAudioSourse.isPlaying)
            {
                if(NoiseDelayCount <= 0)
                {
                    PlayAudioClip(Random.Range(0.1f, 0.5f), 0);
                    NoiseDelayCount = Random.Range(0, 1.5f);
                }
                else if (NoiseDelayCount >= 1)
                {
                    NoiseDelayCount -= Time.deltaTime;
                }
                else
                {
                    NoiseDelayCount -= 1;
                }
            }

            if (CharacterAudioSourseLoop.volume <= OriginVolumeLoop)
            {
                CharacterAudioSourseLoop.volume = counter * 0.2f;
            }
            if (CharacterAudioSourseLoop.pitch < 1.6f)
                CharacterAudioSourseLoop.pitch += Time.deltaTime*2;
            if (!CharacterAudioSourseLoop.isPlaying&&quick ==false)
            {
                CharacterAudioSourseLoop.PlayOneShot(AudioCharacter.AudioClip[1]);              
            }
            

            vignette.active = true;
            float targetIntensity = vignetteAmplitude * Mathf.Sin(Time.time * vignetteSpeed + NoiseTime) + 0.4f;
            vignetteIntensity += Time.deltaTime;
            vignette.intensity.Override(vignetteIntensity);
            if (vignetteIntensity > targetIntensity)
            {
                State = 1;
            }

            float targetSaturation = 0.5f + saturationAmplitude * Mathf.Sin(Time.time * saturationSpeed + NoiseTime);
            saturationValue += Time.deltaTime;
            Color color = vignette.color.value;
            color.r = saturationValue;
            vignette.color.Override(color);
            if (saturationValue > targetSaturation)
            {
                State = 1;
            }
        }
        else if(State == 1)
        {
            counter += Time.deltaTime;

            if (Random.Range(0, 10) > 8)
            {
                NoiseTime += Random.Range(-1f, 0.1f);
            }
            render.material.SetFloat("_Strength", Random.Range(NoiseMaxStrenth / 1.5f, NoiseMaxStrenth));


            if (!CharacterAudioSourse.isPlaying)
            {
                if (NoiseDelayCount <= 0)
                {
                    
                    PlayAudioClip(Random.Range(0.1f, 0.5f), 0);
                    NoiseDelayCount = Random.Range(0, 1.5f);
                }
                else if (NoiseDelayCount >= 1)
                {
                    NoiseDelayCount -= Time.deltaTime;
                }
                else
                {
                    NoiseDelayCount -= 1;
                }
            }
            if (!CharacterAudioSourseLoop.isPlaying&&quick == false)
            {
                CharacterAudioSourseLoop.PlayOneShot(AudioCharacter.AudioClip[1]);
            }

            vignetteIntensity = vignetteAmplitude * Mathf.Sin(Time.time * vignetteSpeed + NoiseTime) + 0.4f;
            vignette.intensity.Override(vignetteIntensity);

            saturationValue = 0.5f + saturationAmplitude * Mathf.Sin(Time.time * saturationSpeed + NoiseTime);
            Color color = vignette.color.value;
            color.r = saturationValue;
            vignette.color.Override(color);

        }
        else if(State == 2)
        {
            CharacterAudioSourse.volume -= Time.deltaTime * 0.2f;
            CharacterAudioSourseLoop.volume -= Time.deltaTime * 0.1f;

            vignetteIntensity -= Time.deltaTime*0.3f;
            vignette.intensity.Override(vignetteIntensity);

            saturationValue -= Time.deltaTime;
            Color color = vignette.color.value;
            color.r = saturationValue;
            vignette.color.Override(color);

            if (!CharacterAudioSourse.isPlaying)
            {
                if (NoiseDelayCount <= 0)
                {

                    PlayAudioClip(Random.Range(0.1f, 0.3f), 0);
                    NoiseDelayCount = Random.Range(0, 1.5f);
                }
                else if (NoiseDelayCount >= 1)
                {
                    NoiseDelayCount -= Time.deltaTime;
                }
                else
                {
                    NoiseDelayCount -= 1;
                }
            }

            if(CharacterAudioSourseLoop.pitch > 1.0f)
            {
                CharacterAudioSourseLoop.pitch -= Time.deltaTime;
            }
            if (!CharacterAudioSourseLoop.isPlaying&& quick == false)
            {
                CharacterAudioSourseLoop.PlayOneShot(AudioCharacter.AudioClip[1]);
            }

            render.material.SetFloat("_Strength", Mathf.Clamp((render.material.GetFloat("_Strength") - Time.deltaTime * 0.2f),0,NoiseMaxStrenth));

            if (vignetteIntensity <= 0 && saturationValue <= 0)
            {
                State = 3;
                render.material.SetFloat("_Strength", 0);
                NoiseAffect.SetActive(false);
                saturationValue = 0;
                vignetteIntensity = 0;
                CharacterAudioSourse.SetScheduledEndTime(AudioSettings.dspTime);
                CharacterAudioSourseLoop.Stop();
                CharacterAudioSourse.volume = OriginVolume;
                CharacterAudioSourseLoop.volume = OriginVolumeLoop;
                CharacterAudioSourseLoop.pitch = 1;
                vignette.active = false;
            }
        }
    }
    void PlayAudioClip(float duration,int clip)
    {
        CharacterAudioSourse.clip = AudioCharacter.AudioClip[clip];
        CharacterAudioSourse.volume = Random.Range(OriginVolume, OriginVolume + 0.2f);
        CharacterAudioSourse.PlayScheduled(AudioSettings.dspTime); 
        CharacterAudioSourse.SetScheduledEndTime(AudioSettings.dspTime + duration); 
    }
    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        Debug.Log("Scene changed from: " + previousScene.name + " to: " + newScene.name);
        // Perform actions or preparations here
        // For example, reset player position, save game progress, etc.
    }
    private void FindAudioSourse()
    {
        CharacterAudioSourse = GameObject.Find("Character").transform.Find("PlayerAudio").GetComponent<AudioSource>();
        AudioCharacter = CharacterAudioSourse.gameObject.GetComponent<AudioCharacter>();
        CharacterAudioSourseLoop = AudioCharacter.AudioSources[1];
        OriginVolume = CharacterAudioSourse.volume;
        OriginVolumeLoop = CharacterAudioSourseLoop.volume;

        NoiseAffect = Camera.main.gameObject.transform.Find("Plane").gameObject;
        render = NoiseAffect.GetComponent<Renderer>();
    }

}
