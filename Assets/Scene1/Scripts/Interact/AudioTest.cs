using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioSource audioSource;
    private void Awake()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
