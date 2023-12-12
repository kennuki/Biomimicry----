using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class BrightnessAdjust : MonoBehaviour
{
    public Slider adjustmentSlider;
    public Volume postProcessVolume;
    private VolumeProfile postProcessProfile;
    private VolumeProfile clonedProfile;
    private ColorAdjustments colorAdjustments;
    private float initialExposure;
    private float initialContrast;
    private float initialSaturation;

    private void Start()
    {
        adjustmentSlider.value = Setting.Brightness;
        postProcessProfile = postProcessVolume.profile;
        clonedProfile = Instantiate(postProcessProfile);
        if (clonedProfile.TryGet(out colorAdjustments))
        {
            initialExposure = colorAdjustments.postExposure.value;
            initialContrast = colorAdjustments.contrast.value;
            initialSaturation = colorAdjustments.saturation.value;

            adjustmentSlider.onValueChanged.AddListener(AdjustPostProcessingProperties);
        }
    }
    private void AdjustPostProcessingProperties(float value)
    {
        colorAdjustments.postExposure.Override(initialExposure + value);

        colorAdjustments.contrast.Override(initialContrast - value*1.5f);

        colorAdjustments.saturation.Override(initialSaturation - value);

        postProcessVolume.profile = clonedProfile;
        Setting.Brightness = adjustmentSlider.value;
    }
}
