using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVoice : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] audioClips;
    Rigidbody rb;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    float Volume = 1;
    private void Update()
    {
        Volume = Mathf.Clamp(rb.velocity.magnitude / 20,0f,1f);
    }
    private void OnCollisionStay(Collision collision)
    {
        int clip = Random.Range(0, 3);

        if (rb.velocity.magnitude > 0.1f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClips[clip], Volume);
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        audioSource.Stop();
    }

}
