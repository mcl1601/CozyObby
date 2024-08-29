
using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRC.SDKBase;
using VRC.Udon;

public class VolumeCheck : UdonSharpBehaviour
{
    [Header("--- System ---")]
    [SerializeField] private UdonBehaviour resetController;
    [SerializeField] private UnityEngine.UI.Slider relatedSlider;

    [Header("--- Variables ---")]
    [Tooltip("This should be 'effect name' (in lowercase) and then 'Value'")]
    [SerializeField] private string variableName;

    [Tooltip("This flips the outputted value to be negative. \nThis is for if the script is on an 'effect-Low' object")]
    [SerializeField] private bool isLowType;

    private PostProcessVolume ppeVolume;
    private float defaultWeight = 0f;

    void Start()
    {
        ppeVolume = this.GetComponent<PostProcessVolume>();

        if (ppeVolume.weight > 0f)
        {
            GetVolume();
        }
    }

    public void GetVolume()
    {
        defaultWeight = ppeVolume.weight;

        if (isLowType)
        {
            relatedSlider.value = defaultWeight * relatedSlider.maxValue * -1;

            resetController.SetProgramVariable(variableName, defaultWeight * -1);
        }
        else
        {
            relatedSlider.value = defaultWeight * relatedSlider.maxValue;

            resetController.SetProgramVariable(variableName, defaultWeight);
        }
    }
}