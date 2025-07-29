
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Persistence;
using UnityEngine.UI;
using System;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WorldToggles : UdonSharpBehaviour
{
    [SerializeField] GameObject colliders, audioLink, pens, stringLights;
    [SerializeField] GameObject[] crowns, particles, lightVolumes;
    [SerializeField] AudioClip click;
    [SerializeField] Toggle crown, col, al, pen, sl, part, lv;
    private AudioSource clickSrc;

    #region Persistence Keys
    private const string CROWN_KEY = "crowns";
    private const string PARTICLE_KEY = "particles";
    private const string AL_KEY = "al";
    private const string SL_KEY = "sl";
    private const string COL_KEY = "col";
    private const string PENS_KEY = "pens";
    private const string LV_KEY = "lv";
    #endregion
    void Start()
    {
        clickSrc = GetComponent<AudioSource>();
    }

    public override void OnPlayerRestored(VRCPlayerApi player)
    {
        if(Networking.LocalPlayer == player)
        {
            if(PlayerData.HasKey(player, CROWN_KEY))
            {
                crown.isOn = PlayerData.GetBool(player, CROWN_KEY);
            }
            if(PlayerData.HasKey(player, PARTICLE_KEY))
            {
                part.isOn = PlayerData.GetBool(player, PARTICLE_KEY);
            }
            if(PlayerData.HasKey(player, AL_KEY))
            {
                al.isOn = PlayerData.GetBool(player, AL_KEY);
            }
            if(PlayerData.HasKey(player, SL_KEY))
            {
                sl.isOn = PlayerData.GetBool(player, SL_KEY);
            }
            if(PlayerData.HasKey(player, COL_KEY))
            {
                col.isOn = PlayerData.GetBool(player, COL_KEY);
            }
            if(PlayerData.HasKey(player, PENS_KEY))
            {
                pen.isOn = PlayerData.GetBool(player, PENS_KEY);
            }
            if(PlayerData.HasKey(player, LV_KEY))
            {
                lv.isOn = PlayerData.GetBool(player, LV_KEY);
            }
        }
    }

    public void PlayClick()
    {
        clickSrc.PlayOneShot(click);
    }

    public void ToggleColliders()
    {
        colliders.SetActive(!colliders.activeSelf);
        PlayerData.SetBool(COL_KEY, colliders.activeSelf);
        PlayClick();
    }

    public void ToggleAL()
    {
        audioLink.SetActive(!audioLink.activeSelf);
        PlayerData.SetBool(AL_KEY, audioLink.activeSelf);
        PlayClick();
    }

    public void ToggleCrowns()
    {
        bool status = false;
        foreach(GameObject g in crowns)
        {
            g.SetActive(!g.activeSelf);
            status = g.activeSelf;
        }
        PlayerData.SetBool(CROWN_KEY, status);
        PlayClick();
    }

    public void ToggleParticles()
    {
        bool status = false;
        foreach(GameObject g in particles)
        {
            g.SetActive(!g.activeSelf);
            status = g.activeSelf;
        }
        PlayerData.SetBool(PARTICLE_KEY, status);
        PlayClick();
    }

    public void TogglePens()
    {
        pens.SetActive(!pens.activeSelf);
        PlayerData.SetBool(PENS_KEY, pens.activeSelf);
        PlayClick();
    }

    public void ToggleStringLights()
    {
        stringLights.SetActive(!stringLights.activeSelf);
        PlayerData.SetBool(SL_KEY, stringLights.activeSelf);
        PlayClick();
    }

    public void ToggleLightVolumes()
    {
        bool status = false;
        foreach(GameObject g in lightVolumes)
        {
            g.SetActive(!g.activeSelf);
            status = g.activeSelf;
        }
        PlayerData.SetBool(LV_KEY, status);
        PlayClick();
    }
}
