
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class BouncePad : UdonSharpBehaviour
{
    [SerializeField] float strength = 8f;
    private AudioSource audioSrc;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        Vector3 playerVel = Networking.LocalPlayer.GetVelocity();
        Vector3 planer = Vector3.ProjectOnPlane(playerVel, transform.forward);
        playerVel = planer + (transform.forward * strength);
        Networking.LocalPlayer.SetVelocity(playerVel);
        audioSrc.Play();
    }
}
