using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public float pitch = 100;
    public string pitch_name = "HeartPitch"; 

    public void SetPitch(float pitchValue)
    {

            audioMixer.SetFloat(pitch_name, pitchValue);


    }
    private void Update()
    {
        SetPitch(pitch);
    }
}
