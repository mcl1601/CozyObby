
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MuffleSoundZone : UdonSharpBehaviour
{
    private MuffleSoundManager manager;
    [SerializeField] private bool isTwo;

    void Start()
    {
        manager = transform.parent.GetComponent<MuffleSoundManager>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player == Networking.LocalPlayer)
        {
            if(isTwo)
            {
                manager.isOn2 = true;
            }
            else manager.isOn = true;

            manager.CheckValues();
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if(player == Networking.LocalPlayer)
        {
            if(isTwo)
            {
                manager.isOn2 = false;
            }
            else manager.isOn = false;

            manager.CheckValues();
        }
    }
}
