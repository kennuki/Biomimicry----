using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LandSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClip;
    public CharacterController controller;
    float c = 0;
    int playstate = 0;
    private void Update()
    {
        //Debug.Log(playstate);
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



    }

    string type;

    private void OnTriggerStay(Collider other)
    {
        if (playstate==1)
        {
            if (other.gameObject.GetComponent<Text>() != null)
            {
                type = other.GetComponent<Text>().text;
            }

            switch (type)
            {
                case "Metal":
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.PlayOneShot(audioClip[0]);
                            playstate = 2;
                        }
                        break;
                    }
                case "Wood":
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.PlayOneShot(audioClip[1]);
                            playstate = 2;
                        }
                        break;
                    }
                case "Tile":
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.PlayOneShot(audioClip[2]);
                            playstate = 2;
                        }
                        break;
                    }
                case "Carpet":
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.PlayOneShot(audioClip[3]);
                            playstate = 2;
                        }
                        break;
                    }


                default:
                    break;
            }
        }
       
    }

}
