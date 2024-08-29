
using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRC.SDKBase;
using VRC.Udon;

public class MenuController : UdonSharpBehaviour
{
    [Header("--- Sliders ---")]
    [SerializeField] private UnityEngine.UI.Slider bloomSlider;
    [SerializeField] private UnityEngine.UI.Slider exposureSlider;
    [SerializeField] private UnityEngine.UI.Slider temperatureSlider;
    [SerializeField] private UnityEngine.UI.Slider tintSlider;
    [SerializeField] private UnityEngine.UI.Slider contrastSlider;
    [SerializeField] private UnityEngine.UI.Slider saturationSlider;

    [Header("--- Post-Process Volumes ---")]
    [SerializeField] private PostProcessVolume bloomVolume;
    [SerializeField] private PostProcessVolume exposureHighVolume;
    [SerializeField] private PostProcessVolume exposureLowVolume;
    [SerializeField] private PostProcessVolume temperatureWarmVolume;
    [SerializeField] private PostProcessVolume temperatureCoolVolume;
    [SerializeField] private PostProcessVolume tintRedVolume;
    [SerializeField] private PostProcessVolume tintGreenVolume;
    [SerializeField] private PostProcessVolume contrastHighVolume;
    [SerializeField] private PostProcessVolume contrastLowVolume;
    [SerializeField] private PostProcessVolume saturationHighVolume;
    [SerializeField] private PostProcessVolume saturationLowVolume;

    // Calls every event on start, so that the sliders and post process volumes get set accordingly
    void Start()
    {
        Bloom();
        Exposure();
        Temperature();
        Contrast();
        Tint();
        Saturation();
    }

    // Bloom controlling code
    public void Bloom()
    {
        if (bloomSlider.value == bloomSlider.minValue)
        {
            bloomVolume.gameObject.SetActive(false);
        }
        else
        {
            bloomVolume.gameObject.SetActive(true);

            bloomVolume.weight = bloomSlider.value / bloomSlider.maxValue;
        }
    }

    // Exposure controlling code
    public void Exposure()
    {
        float expSliderValue = exposureSlider.value;

        if (expSliderValue == 0)
        {
            exposureLowVolume.gameObject.SetActive(false);
            exposureHighVolume.gameObject.SetActive(false);
        }
        else
        {
            bool greaterThan0 = expSliderValue > 0;

            exposureLowVolume.gameObject.SetActive(!greaterThan0);
            exposureHighVolume.gameObject.SetActive(greaterThan0);

            if (greaterThan0)
            {
                exposureHighVolume.weight = expSliderValue / exposureSlider.maxValue;
            }
            else
            {
                exposureLowVolume.weight = expSliderValue / exposureSlider.minValue;
            }
        }
    }

    // Temperature controlling code
    public void Temperature()
    {
        float tmpSliderValue = temperatureSlider.value;

        if (tmpSliderValue == 0)
        {
            temperatureCoolVolume.gameObject.SetActive(false);
            temperatureWarmVolume.gameObject.SetActive(false);
        }
        else
        {
            bool greaterThan0 = tmpSliderValue > 0;

            temperatureCoolVolume.gameObject.SetActive(!greaterThan0);
            temperatureWarmVolume.gameObject.SetActive(greaterThan0);

            if (greaterThan0)
            {
                temperatureWarmVolume.weight = tmpSliderValue / temperatureSlider.maxValue;
            }
            else
            {
                temperatureCoolVolume.weight = tmpSliderValue / temperatureSlider.minValue;
            }
        }
    }

    // Contrast controlling code
    public void Contrast()
    {
        float contSliderValue = contrastSlider.value;

        if (contSliderValue == 0)
        {
            contrastLowVolume.gameObject.SetActive(false);
            contrastHighVolume.gameObject.SetActive(false);
        }
        else
        {
            bool greaterThan0 = contSliderValue > 0;

            contrastLowVolume.gameObject.SetActive(!greaterThan0);
            contrastHighVolume.gameObject.SetActive(greaterThan0);

            if (greaterThan0)
            {
                contrastHighVolume.weight = contSliderValue / contrastSlider.maxValue;
            }
            else
            {
                contrastLowVolume.weight = contSliderValue / contrastSlider.minValue;
            }
        }
    }

    // Tint controlling code
    public void Tint()
    {
        float tintSliderValue = tintSlider.value;

        if (tintSliderValue == 0)
        {
            tintGreenVolume.gameObject.SetActive(false);
            tintRedVolume.gameObject.SetActive(false);
        }
        else
        {
            bool greaterThan0 = tintSliderValue > 0;

            tintGreenVolume.gameObject.SetActive(!greaterThan0);
            tintRedVolume.gameObject.SetActive(greaterThan0);

            if (greaterThan0)
            {
                tintRedVolume.weight = tintSliderValue / tintSlider.maxValue;
            }
            else
            {
                tintGreenVolume.weight = tintSliderValue / tintSlider.minValue;
            }
        }
    }

    // Saturation controlling code
    public void Saturation()
    {
        float satSliderValue = saturationSlider.value;

        if (satSliderValue == 0)
        {
            saturationLowVolume.gameObject.SetActive(false);
            saturationHighVolume.gameObject.SetActive(false);
        }
        else
        {
            bool greaterThan0 = satSliderValue > 0;

            saturationLowVolume.gameObject.SetActive(!greaterThan0);
            saturationHighVolume.gameObject.SetActive(greaterThan0);

            if (greaterThan0)
            {
                saturationHighVolume.weight = satSliderValue / saturationSlider.maxValue;
            }
            else
            {
                saturationLowVolume.weight = satSliderValue / saturationSlider.minValue;
            }
        }
    }
}
