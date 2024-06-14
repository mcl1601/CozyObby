
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TimerResetter : UdonSharpBehaviour
{
    private ObbyManager manager;
    void Start()
    {
        manager = transform.parent.GetComponent<ObbyManager>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        manager.ResetTimer();
    }
}
