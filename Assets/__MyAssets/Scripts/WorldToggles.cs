
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WorldToggles : UdonSharpBehaviour
{
    [SerializeField] GameObject colliders, audioLink, pens, stringLights;
    [SerializeField] GameObject[] crowns, particles;
    [SerializeField] AudioClip click;
    private AudioSource clickSrc;
    void Start()
    {
        clickSrc = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        clickSrc.PlayOneShot(click);
    }

    public void ToggleColliders()
    {
        colliders.SetActive(!colliders.activeSelf);
        PlayClick();
    }

    public void ToggleAL()
    {
        audioLink.SetActive(!audioLink.activeSelf);
        PlayClick();
    }

    public void ToggleCrowns()
    {
        foreach(GameObject g in crowns)
        {
            g.SetActive(!g.activeSelf);
        }
        PlayClick();
    }

    public void ToggleParticles()
    {
        foreach(GameObject g in particles)
        {
            g.SetActive(!g.activeSelf);
        }
        PlayClick();
    }

    public void TogglePens()
    {
        pens.SetActive(!pens.activeSelf);
        PlayClick();
    }

    public void ToggleStringLights()
    {
        stringLights.SetActive(!stringLights.activeSelf);
        PlayClick();
    }
}
