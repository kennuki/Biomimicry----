using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LandSound : MonoBehaviour
{
    public AudioSource audioSource;
    public CharacterController controller;
    private AudioClip clip;
    float initialVolume;
    float c = 0;
    int playstate = 0;
    float maxspeed;
    private void Update()
    {
        if (!controller.isGrounded)
        {
            if (controller.velocity.y > maxspeed)
                maxspeed = controller.velocity.y;
        }
        UngroundTime();
        landsound();
    }
    private void Start()
    {
        initialVolume = audioSource.volume;
    }
    private void UngroundTime()
    {
        if (controller.isGrounded == true)
        {
            c = 0;
            if(playstate == 2)
            {
                playstate = 0;
            }
        }

        else
            c += Time.deltaTime;
    }
    private void landsound()
    {
        if(playstate == 0)
        {
            if (controller.isGrounded == false && c > 0.5f)
            {
                playstate = 1;
            }
        }
        if(playstate == 1)
        {
            PlayAudio();
        }


    }
    private bool PerformRaycast()
    {
        Vector3 OriginPos = transform.position;
        RaycastHit hit;
        FloorType floor;
        if (Physics.Raycast(OriginPos, Vector3.down, out hit, 2))
        {
            floor = hit.transform.GetComponent<FloorType>();
            if (floor != null)
            {
                clip = FloorTypeInfo.GetAudioClipLand(floor.floorType);
                return true;
            }
            return false;
        }
        return false;
    }
    private void PlayAudio()
    {
        if (PerformRaycast())
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.02f+initialVolume * maxspeed * 0.2f;
                maxspeed = 0;
                audioSource.clip = clip;
                audioSource.Play();
                playstate = 2;
            }
        }
    }




}
