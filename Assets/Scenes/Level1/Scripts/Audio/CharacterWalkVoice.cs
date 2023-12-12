using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterWalkVoice : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioMixer audioMixer;
    public float AudioSpeed;
    public float fadeDuration = 0.05f;
    public float StopTime = 0.05f;

    private GameObject character;
    private CharacterController controller;
    private AudioClip clip;
    private float startVolume;
    private float mouse_input_x, mouse_input_y;

    private void Start()
    {
        startVolume = audioSource.volume;
        character = GameObject.Find("Character");
        controller = character.GetComponent<CharacterController>();
    }
    private void Update()
    {
        AudioControl();
        DetectAllow();
        DetectFloor();
    }
    private void AudioControl()
    {
        AudioSpeed = Character.speed * 0.46f;
        audioSource.pitch = Mathf.Clamp(AudioSpeed, 0.8f, 2);
        mouse_input_x = Input.GetAxis("Horizontal");
        mouse_input_y = Input.GetAxis("Vertical");
    }
    private void DetectFloor()
    {
        if (DetectAllow())
            PlayAudio();
        else
            StartCoroutine(FadeVolume());
    }
    private bool PerformRaycast()
    {
        Vector3 OriginPos = transform.position;
        RaycastHit hit;
        FloorType floor;
        if (Physics.Raycast(OriginPos, Vector3.down, out hit, 2,7))
        {
            floor = hit.transform.GetComponent<FloorType>();
            if (floor != null)
            {
                clip = FloorTypeInfo.GetAudioClipWalk(floor.floorType);
                return true;
            }
            return false;
        }
        return false;
    }
    private bool DetectAllow()
    {
        if (!controller.isGrounded)
            return false;
        if (mouse_input_x == 0 && mouse_input_y == 0)
            return false;
        if (controller.enabled == false)
            return false;
        return true;
    }
    private void PlayAudio()
    {
        if (PerformRaycast())
        {
            audioSource.clip = clip;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            StartCoroutine(FadeVolume());
        }
    }
    private IEnumerator FadeVolume()
    {
        float t = 0;
        while (t < fadeDuration && !controller.isGrounded)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.Pause();
        while (!DetectAllow())
        {
            t += Time.deltaTime;
            if (t > StopTime)
            {
                audioSource.Stop();
                audioSource.volume = startVolume;
                yield break;
            }
            yield return null;
        }
        audioSource.volume = startVolume;
        yield break;
    }
}
