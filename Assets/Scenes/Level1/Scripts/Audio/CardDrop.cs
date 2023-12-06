using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrop : MonoBehaviour
{
    public AudioClip[] collisionSound1;

    private Rigidbody rb;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    private float volume;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int index = Random.Range(0, collisionSound1.Length);
        if(this.gameObject.transform.parent != null)
        {
            if (!audioSource1.isPlaying)
            {
                audioSource1.clip = collisionSound1[index];
                audioSource1.Play();
            }
            else if (!audioSource2.isPlaying)
            {
                audioSource1.clip = collisionSound1[index];
                audioSource2.Play();
            }

        }

    }

    private void speed_to_volume()
    {
        volume = rb.velocity.magnitude/3;
        volume = Mathf.Clamp(volume, 0.4f, 2);
        audioSource1.volume = volume;
        audioSource2.volume = volume;
    }
    private void Update()
    {
        speed_to_volume();
    }
}
