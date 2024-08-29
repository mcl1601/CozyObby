
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SettingsManager : UdonSharpBehaviour
{
    [SerializeField] Toggle toggles, pp, credits;
    [SerializeField] GameObject g_Toggle, g_PP, g_Credits;
    [SerializeField] WorldToggles manager;
    void Start()
    {
    }

    public void Toggles()
    {
        if(!toggles.isOn)
        {
            toggles.SetIsOnWithoutNotify(true);
            return;
        }

        toggles.SetIsOnWithoutNotify(true);
        pp.SetIsOnWithoutNotify(false);
        credits.SetIsOnWithoutNotify(false);
        g_Toggle.SetActive(true);
        g_PP.SetActive(false);
        g_Credits.SetActive(false);
        manager.PlayClick();
    }

    public void PP()
    {
        if(!pp.isOn)
        {
            pp.SetIsOnWithoutNotify(true);
            return;
        }

        toggles.SetIsOnWithoutNotify(false);
        pp.SetIsOnWithoutNotify(true);
        credits.SetIsOnWithoutNotify(false);
        g_Toggle.SetActive(false);
        g_PP.SetActive(true);
        g_Credits.SetActive(false);
        manager.PlayClick();
    }

    public void Credits()
    {
        if(!credits.isOn)
        {
            credits.SetIsOnWithoutNotify(true);
            return;
        }

        toggles.SetIsOnWithoutNotify(false);
        pp.SetIsOnWithoutNotify(false);
        credits.SetIsOnWithoutNotify(true);
        g_Toggle.SetActive(false);
        g_PP.SetActive(false);
        g_Credits.SetActive(true);
        manager.PlayClick();
    }
}
