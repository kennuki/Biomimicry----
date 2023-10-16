using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDropVoice : MonoBehaviour
{
    AudioSource Audio;
    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }
    public void PlayAudio()
    {
        Audio.Play();
    }
}
