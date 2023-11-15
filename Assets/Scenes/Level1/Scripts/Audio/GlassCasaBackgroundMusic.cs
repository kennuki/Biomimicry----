using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCasaBackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private void Start()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
