
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SliderValueDisplay : UdonSharpBehaviour
{
    [SerializeField] private UnityEngine.UI.Text valueText;
    [SerializeField] private UnityEngine.UI.Slider selfSlider;

    void Start()
    {
        ValueChanged();
    }

    public void ValueChanged()
    {
        valueText.text = selfSlider.value.ToString();
    }
}
