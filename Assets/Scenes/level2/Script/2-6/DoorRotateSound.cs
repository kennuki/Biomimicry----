using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotateSound : MonoBehaviour
{
    public Rigidbody targetRigidbody;
    public AudioSource audioSource;
    public float minForceThreshold = 0.1f;
    public float maxForceThreshold = 10.0f;
    public float minVolume = 0.1f;
    public float maxVolume = 1.0f;

    void Update()
    {
        if (targetRigidbody != null && audioSource != null)
        {
            // 获取目标物体的旋转力量
            float rotationForce = targetRigidbody.angularVelocity.magnitude;

            // 将旋转力量映射到音效大小范围
            float mappedVolume = Mathf.Clamp01(Mathf.InverseLerp(minForceThreshold, maxForceThreshold, rotationForce));

            // 根据力量调整音效大小
            float volume = Mathf.Lerp(minVolume, maxVolume, mappedVolume);
            audioSource.volume = volume;


            // 在这里添加其他处理音效的逻辑，比如播放音效
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}
