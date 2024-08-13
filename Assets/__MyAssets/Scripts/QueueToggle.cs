
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class QueueToggle : UdonSharpBehaviour
{
    [SerializeField] Animator anim;
    VRCPlayerApi lp;
    void Start()
    {
        lp = Networking.LocalPlayer;
        anim.Play("Out");
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player == lp)
        {
            anim.Play("In");
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if(player == lp)
        {
            anim.Play("Out");
        }
    }
}
