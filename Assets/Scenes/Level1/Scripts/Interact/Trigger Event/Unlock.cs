using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    public GameObject chair;
    public EventActive active;
    public PosRotAdjust adjust;
    public StageRoutine routine;
    public AudioSource source;
    public AudioSource source2;
    public AudioClip[] clip;
    private void Update()
    {
        if (active.Active)
        {
            WhenActive();
        }
        if (adjust.arrive)
        {
            StartCoroutine(soundStart());
            active.ContinuePlayerAction = true;
            this.enabled = false;
        }
    }

    private void WhenActive()
    {
        if (chair != null)
            chair.SetActive(true);
        adjust.enabled = true;
        routine.enabled = true;
    }
    private IEnumerator soundStart()
    {
        yield return new WaitForSeconds(0.8f);
        source.PlayOneShot(clip[0],2f);
        yield return new WaitForSeconds(0.2f);
        source.clip = clip[1];
        source.loop = true;
        StartCoroutine(smoothsound());
        source.Play();
        yield return new WaitForSeconds(5f);
        source2.PlayOneShot(clip[4]);
    }
    private IEnumerator smoothsound()
    {
        float initial = source.volume;
        for(float timer = 0; timer < 1.5f; timer += Time.deltaTime)
        {
            source.volume = timer * initial / 1.5f;
            yield return null;
        }
        source.volume = initial;
    }
}
