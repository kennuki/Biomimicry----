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

    float c = 0;
    int playstate = 0;
    private void Update()
    {
        UngroundTime();
        landsound();
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
                audioSource.clip = clip;
                audioSource.Play();
                playstate = 2;
            }
        }
    }




}
