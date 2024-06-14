
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TimerGate : UdonSharpBehaviour
{
    [SerializeField] bool isFinish;
    private ObbyManager manager;
    void Start()
    {
        manager = transform.parent.GetComponent<ObbyManager>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        if(isFinish)
        {
            manager.StopTimer();
        }
        else
        {
            manager.StartTimer();
        }
    }
}
