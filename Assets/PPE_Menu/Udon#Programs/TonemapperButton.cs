
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TonemapperButton : UdonSharpBehaviour
{
    [Header("--- Post-Process Volumes ---")]
    [SerializeField] private GameObject tonemapperCustom;
    [SerializeField] private GameObject tonemapperNeutral;
    [SerializeField] private GameObject tonemapperACES;

    [Header("--- Text ---")]
    [Tooltip("The text that will display the current tonemapper")]
    [SerializeField] private UnityEngine.UI.Text modeText;

    [Header("--- Default Type ---")]
    [Tooltip("0 = default tonemapper 'Custom' \n1 = default tonemapper 'Neutral' \n2 = default tonemapper 'ACES'")]
    [Range(0, 2)] [SerializeField] private int defaultTonemapper = 0;

    int i = 0;

    void Start()
    {
        i = defaultTonemapper;
        OnClick();
    }

    public void OnClick()
    {
        switch (i)
        {
            default:
                tonemapperCustom  .SetActive(true);
                tonemapperNeutral .SetActive(false);
                tonemapperACES    .SetActive(false);

                modeText.text = "Default";
                break;
            case 1:
                tonemapperCustom  .SetActive(false);
                tonemapperNeutral .SetActive(true);

                modeText.text = "Neutral";
                break;
            case 2:
                tonemapperNeutral .SetActive(false);
                tonemapperACES    .SetActive(true);

                modeText.text = "ACES";
                break;
        }
        i++;
        if (i > 2) i = 0;
    }
}
