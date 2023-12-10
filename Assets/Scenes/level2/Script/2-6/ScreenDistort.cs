using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenDistort : MonoBehaviour
{
    public Volume postProcessVolume;
    private VolumeProfile postProcessProfile;
    private LensDistortion distortion;
    private ChromaticAberration aberration;
    private FilmGrain filmGrain;
    private void Start()
    {
        postProcessVolume = gameObject.GetComponent<Volume>();
        postProcessVolume.isGlobal = true;
        postProcessProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        postProcessVolume.sharedProfile = postProcessProfile;
        postProcessProfile.Add<LensDistortion>();
        postProcessProfile.Add<ChromaticAberration>();
        postProcessProfile.Add<FilmGrain>();
        postProcessProfile.TryGet(out distortion);
        postProcessProfile.TryGet(out aberration);
        postProcessProfile.TryGet(out filmGrain);
        distortion.active = false;
        aberration.active = false;
        filmGrain.active = false;
    }
    public IEnumerator TransitionPostProcessing()
    {
        float duration = 2f; 
        float timer = 0f;
        aberration.active = true;
        filmGrain.active = true;
        distortion.active = true;
        aberration.intensity.overrideState = true;
        filmGrain.intensity.overrideState = true;
        distortion.intensity.overrideState = true;
        filmGrain.type.overrideState = true;
        filmGrain.type.value = FilmGrainLookup.Large02;
        while (timer < duration)
        {
            float progress = timer / duration;

            aberration.intensity.value = Mathf.Lerp(0f, 1f, progress);
            filmGrain.intensity.value = Mathf.Lerp(0f, 1f, progress);
            distortion.intensity.value = Mathf.Lerp(0f, 1f, progress*0.4f);

            timer += Time.deltaTime;
            yield return null;
        }

    }
    public IEnumerator DistortPingPong()
    {
        float timer = 0f;
        distortion.active = true;
        distortion.xMultiplier.overrideState = true;
        distortion.yMultiplier.overrideState = true;
        while (true)
        {
            distortion.xMultiplier.value = Mathf.PingPong(timer/2,1);
            distortion.yMultiplier.value = Mathf.PingPong(timer/3, 1);
            timer += Time.deltaTime;
            yield return null;
        }

    }

}
