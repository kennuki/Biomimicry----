using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer; 
    public Slider volumeSlider; 

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        volumeSlider.value = audioMixer.GetFloat("MasterVolume", out float volume) ? volume : 0f;
    }

    private void OnVolumeChanged(float value)
    {
        float mappedValue = Mathf.Lerp(-80f, 20f, value);

        audioMixer.SetFloat("MasterVolume", mappedValue);
    }
}
