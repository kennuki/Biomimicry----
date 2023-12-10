using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricCable : MonoBehaviour
{
    public ParticleSystem particle;
    public AudioClip soundEffect;

    private ParticleSystem.Burst[] bursts;
    public AudioSource audioSource;
    private float time = 0;

    void Start()
    {
        // 確保有指定 ParticleSystem 和 AudioClip
        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
        }

        audioSource.clip = soundEffect;

        // 取得粒子系統的所有爆發設定
        bursts = new ParticleSystem.Burst[particle.emission.burstCount];
        particle.emission.GetBursts(bursts);
    }

    void Update()
    {
        // 在這裡添加檢測粒子產生的條件，例如根據時間、事件等
        time = particle.time;
        // 在指定時間播放音效
        PlaySoundAtBurstTime(1.0f); // 這裡示範在爆發時間 1.0 秒時播放音效
    }

    void PlaySoundAtBurstTime(float burstTime)
    {
        foreach (ParticleSystem.Burst burst in bursts)
        {
            // 判斷是否有爆發發生在指定時間
            if (burst.time < time && burst.time > time - 0.1f)
            {
                audioSource.Play();
                break;
            }
        }
    }
}
