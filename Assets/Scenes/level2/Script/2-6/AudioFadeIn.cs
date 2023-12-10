using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeInDuration = 3f; // Fade In 持續時間

    private float startVolume;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            startVolume = audioSource.volume;
            audioSource.volume = 0f; // 初始音量設置為 0，即 Fade In 開始時完全靜音
            PlayFadeIn();
        }
        else
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
        }
    }

    void PlayFadeIn()
    {
        if (audioSource != null)
        {
            // 使用協程實現 Fade In
            StartCoroutine(FadeIn());
        }
    }

    System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            // 線性插值計算音量值
            audioSource.volume = Mathf.Lerp(0f, startVolume, elapsedTime / fadeInDuration);

            // 更新過去的時間
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 確保最終音量設置為開始音量
        audioSource.volume = startVolume;
    }

}
