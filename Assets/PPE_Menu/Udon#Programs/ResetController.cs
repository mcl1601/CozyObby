
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class ResetController : UdonSharpBehaviour
{
    [Header("--- Menu Udonbehaviour ---")]
    [SerializeField] private UdonBehaviour menu;

    [Header("--- Sliders ---")]
    [SerializeField] private UnityEngine.UI.Slider bloomSlider;
    [SerializeField] private UnityEngine.UI.Slider exposureSlider;
    [SerializeField] private UnityEngine.UI.Slider temperatureSlider;
    [SerializeField] private UnityEngine.UI.Slider tintSlider;
    [SerializeField] private UnityEngine.UI.Slider contrastSlider;
    [SerializeField] private UnityEngine.UI.Slider saturationSlider;

    // These are all hidden, because they only need to be accessed by other scripts, and not the user
    // They're also public so that other scripts can change them
    [HideInInspector] public float bloomValue;
    [HideInInspector] public float exposureValue;
    [HideInInspector] public float temperatureValue;
    [HideInInspector] public float tintValue;
    [HideInInspector] public float contrastValue;
    [HideInInspector] public float saturationValue;

    public void ResetAll()
    {
        // Sets all the sliders back to the default values, which were set by the individual scripts on each post process volume on start
        bloomSlider.value       = bloomValue * bloomSlider.maxValue;
        exposureSlider.value    = exposureValue * exposureSlider.maxValue;
        temperatureSlider.value = temperatureValue * temperatureSlider.maxValue;
        tintSlider.value        = tintValue * tintSlider.maxValue;
        contrastSlider.value    = contrastValue * contrastSlider.maxValue;
        saturationSlider.value  = saturationValue * saturationSlider.maxValue;

        // Sends the custom events to the Menu script, so that the post process volumes update along with the sliders
        menu.SendCustomEvent("Bloom");
        menu.SendCustomEvent("Exposure");
        menu.SendCustomEvent("Temperature");
        menu.SendCustomEvent("Tint");
        menu.SendCustomEvent("Contrast");
        menu.SendCustomEvent("Saturation");
    }
}
