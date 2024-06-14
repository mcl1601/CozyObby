
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DeathPlane : UdonSharpBehaviour
{
    private ObbyManager manager;
    private AudioSource audioSource;
    [SerializeField] bool hasSound = true;
    void Start()
    {
        manager = transform.parent.GetComponent<ObbyManager>();
        if(hasSound) audioSource = GetComponent<AudioSource>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        manager.RespawnPlayer();
        if(hasSound) audioSource.Play();
    }
}
