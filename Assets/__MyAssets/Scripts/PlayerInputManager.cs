
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerInputManager : UdonSharpBehaviour
{
    public float vertMove = 0f;
    public float horMove = 0f;
    public VRCPlayerApi.TrackingData head;
    void Start()
    {
        
    }

    public override void InputMoveVertical(float value, UdonInputEventArgs args)
    {
        vertMove = value;
    }

    public override void InputMoveHorizontal(float value, UdonInputEventArgs args)
    {
        horMove = value;
    }

    public override void InputLookHorizontal(float value, UdonInputEventArgs args)
    {
        head = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
    }

    public override void InputLookVertical(float value, UdonInputEventArgs args)
    {
        head = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
    }
}
