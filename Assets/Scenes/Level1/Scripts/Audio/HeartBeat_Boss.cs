using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HeartBeat_Boss : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public string pitch_speed = "HeartPitch";
    public string pitch_pitch = "HeartPitch2";
    public GameObject Boss;
    private Boss boss;
    private float Distance;
    private float rate;
    private float volume;
    private void Start()
    {
        //Boss = GameObject.Find("Boss");
        boss = Boss.GetComponent<Boss>();
    }
    private void Update()
    {
        DisranceToRate();
        heartbeat_calculate();
        if(boss.enabled == true)
        {
            heartbeat_play();
        }
        else
        {
            audioSource.Stop();
        }

    }
    private void DisranceToRate()
    {
        Distance = Vector3.Distance(Boss.transform.position, this.transform.position);
        rate = Mathf.Clamp(5 / Distance, 0.3f, 1.1f);
        volume = Mathf.Clamp(1/Distance-0.04f, 0f, 0.2f);
    }
    private void heartbeat_calculate()
    {
        audioMixer.SetFloat(pitch_speed, (rate+0.3f)*1.5f);
        audioMixer.SetFloat(pitch_pitch, (0.7f/rate)+(0.7f-rate)*0.6f-0.2f);
        audioSource.volume = Mathf.Clamp(volume, 0, 1);
    }
    private void heartbeat_play()
    {
        if (Distance < 25)
        {
            if(!audioSource.isPlaying)
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
