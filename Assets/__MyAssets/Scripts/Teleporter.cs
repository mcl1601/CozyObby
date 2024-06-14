
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Teleporter : UdonSharpBehaviour
{
    private Transform endpoint;
    void Start()
    {
        endpoint = transform.GetChild(0);
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        Networking.LocalPlayer.TeleportTo(endpoint.position, endpoint.rotation);
    }
}
