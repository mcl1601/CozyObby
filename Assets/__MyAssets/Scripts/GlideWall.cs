
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GlideWall : UdonSharpBehaviour
{
    private Vector3 recalculatedVel;
    private bool gliding = false;
    [SerializeField] private float boostVal = 1f;
    private PlayerInputManager input;
    void Start()
    {
        input = transform.parent.GetComponent<PlayerInputManager>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        Vector3 initialVel = player.GetVelocity();
        Vector3 velOnPlane = Vector3.ProjectOnPlane(initialVel, transform.forward).normalized;
        recalculatedVel = velOnPlane * initialVel.magnitude;

        gliding = true;
    }

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        Vector3 headForward = input.head.rotation * Vector3.forward;
        Vector3 headRight = input.head.rotation * Vector3.right;
        recalculatedVel += ((headForward.normalized * input.vertMove) + (headRight * input.horMove)) * Time.deltaTime * boostVal;
        player.SetVelocity(recalculatedVel);
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        Vector3 velOnPlane = Vector3.ProjectOnPlane(recalculatedVel, transform.forward).normalized;
        player.SetVelocity(velOnPlane * recalculatedVel.magnitude);
        gliding = false;
    }

    
}
