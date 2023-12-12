using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAudioAsset : MonoBehaviour
{
    public AudioSource source;
    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip talk;
    public void play_walk1()
    {
        source.PlayOneShot(walk1);
    }
    public void play_walk2()
    {
        source.PlayOneShot(walk2);
    }

}
