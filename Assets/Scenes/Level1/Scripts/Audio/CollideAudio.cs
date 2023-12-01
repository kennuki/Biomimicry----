using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideAudio : MonoBehaviour
{
    public AudioClip[] collisionSound1;

    private Rigidbody rb;
    private AudioSource sample_source;
    public AudioSource[] audioSource;
    public int MaxSoundCount = 3;
    public float max_volume=1;
    public float min_volume=0;
    public float speed_to_volume_rate = 0.1f;
    public float augularSpeed_to_volume_rate = 0.1f;
    private float volume_collide;
    private float volume_angular_collide;
    private float previousVelocity;
    private float previousVelocityChange;
    private float VelocityChangeDiffirence;
    private Vector3 previousAngularVelocity;
    private float previous_angularVelocityDifferenceMagnitude;
    private float Acceleration;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sample_source = GetComponent<AudioSource>();
        audioSource = new AudioSource[MaxSoundCount];
        for(int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i] = gameObject.AddComponent<AudioSource>();
            audioSource[i].spatialBlend = sample_source.spatialBlend;
        }
        previousVelocity = rb.velocity.magnitude;
        previousAngularVelocity = rb.angularVelocity;
        previous_angularVelocityDifferenceMagnitude = 0;
        VelocityChangeDiffirence = 0;
}

    private void OnCollisionEnter(Collision collision)
    {
        AudioPlay();
    }

    private void CalculateAngularForce()
    {
        Vector3 currentAngularVelocity = rb.angularVelocity;

        Vector3 angularVelocityDifference = currentAngularVelocity - previousAngularVelocity;
        float angularVelocityDifferenceMagnitude = angularVelocityDifference.magnitude;
        Acceleration = angularVelocityDifferenceMagnitude - previous_angularVelocityDifferenceMagnitude;
        previous_angularVelocityDifferenceMagnitude = angularVelocityDifferenceMagnitude;
    }
    private void CalculateForced()
    {
        VelocityChangeDiffirence = (rb.velocity.magnitude - previousVelocity) - previousVelocityChange;
        previousVelocity = rb.velocity.magnitude;
        previousVelocityChange = rb.velocity.magnitude - previousVelocity;
    }
    float value;
    private void speed_to_volume()
    {
        volume_collide = rb.velocity.magnitude* speed_to_volume_rate;
        volume_collide = Mathf.Clamp(volume_collide, min_volume, max_volume);
        if (Acceleration < 0)
        {
            volume_angular_collide = -Acceleration * augularSpeed_to_volume_rate;
            volume_angular_collide = Mathf.Clamp(volume_angular_collide, min_volume, max_volume);
        }
        else
        {
            volume_angular_collide = 0;
        }
        value = Mathf.Max(volume_collide, volume_angular_collide);
    }
    private void AudioPlay()
    {
        int index = Random.Range(0, collisionSound1.Length);
        foreach (AudioSource source in audioSource)
        {
            if (source.volume < value)
            {
                source.clip = collisionSound1[index];
                source.volume = value;
                source.PlayOneShot(source.clip);
                break;
            }
            else if (!source.isPlaying)
            {

                source.clip = collisionSound1[index];
                source.volume = value;
                source.PlayOneShot(source.clip);
                break;
            }
        }
    }
    private void Update()
    {
        CalculateAngularForce();
        CalculateForced();
        speed_to_volume();
        if (Acceleration < -0.3f)
        {            
            AudioPlay();
        }
        if (VelocityChangeDiffirence > 0.4f)
        {
            AudioPlay();
        }
    }
}
