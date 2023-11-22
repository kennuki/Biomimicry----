using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CharacterWalkVoice : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] audioClip;
    public AudioMixer audioMixer;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
    }
    public Character character;
    private string floorType;

    public float adjust;
    public float AudioSpeed;
    private void Update()
    {
        AudioSpeed = Character.speed * 0.46f;
        //audioMixer.SetFloat("RunPitch", 1+(1-AudioSpeed)*adjust);
        audioSource.pitch = Mathf.Clamp(AudioSpeed,0.8f,2);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Text>() != null)
            floorType = other.gameObject.GetComponent<Text>().text;
        if (floorType != null&& character.GetComponent<CharacterController>().isGrounded == true)
        {
            if (floorType == "Tile")
            {
                if (character.GetComponent<CharacterController>().velocity.magnitude != 0)
                {
                    if (!audioSource.isPlaying)
                    {
                        if (floorType != null)
                        audioSource.PlayOneShot(audioClip[0]);
                    }
                }
                else
                {
                    StartCoroutine(FadeVolume());
                }
            }
            else if (floorType == "Wood")
            {
                if (character.GetComponent<CharacterController>().velocity.magnitude != 0 )
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClip[1]);
                    }
                }
                else 
                {
                    StartCoroutine(FadeVolume());
                }
            }
            else if (floorType == "Metal")
            {

                if (character.GetComponent<CharacterController>().velocity.magnitude != 0 )
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClip[2]);
                    }
                }
                else
                {
                    StartCoroutine(FadeVolume());
                }
            }
            else if (floorType == "Grass")
            {
                if (character.GetComponent<CharacterController>().velocity.magnitude != 0)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClip[3]);
                    }
                }
                else
                {
                    StartCoroutine(FadeVolume());
                }
            }
            else if (floorType == "Carpet")
            {
                if (character.GetComponent<CharacterController>().velocity.magnitude != 0)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClip[4]);
                    }
                }
                else
                {
                    StartCoroutine(FadeVolume());
                }
            }
        }
        else
        {
            StartCoroutine(FadeVolume());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<Text>() != null)
        {
            if (floorType == other.GetComponent<Text>().text)
            {
                StartCoroutine(FadeVolume());
            }
        }

    }
    float startVolume;
    float fadeDuration = 0.05f;
    private IEnumerator FadeVolume()
    {
        float t = 0;
        floorType = null;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume; 
    }
}
