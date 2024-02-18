using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoorSound : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] clips;
    private void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
    }
    public void DoorSound()
    {
        source.PlayOneShot(clips[0]);
    }
    public void BBSound()
    {
        source.PlayOneShot(clips[1]);
    }
}
