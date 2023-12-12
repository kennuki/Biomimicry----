using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSoldier : MonoBehaviour
{
    public static AudioSoldier instance;
    private void Awake()
    {
        instance = this;
    }
    public AudioSource source;
    public AudioClip lockPlayer;
    public AudioClip kill;
    public AudioClip walk;
    public AudioClip swing;

}
