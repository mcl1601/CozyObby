
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class BounceBall : UdonSharpBehaviour
{
    [SerializeField] float strength = 8f;
    private AudioSource audioSrc;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        Vector3 toPlayer = Networking.LocalPlayer.GetPosition() - transform.position;
        Vector3 playerVel = toPlayer.normalized * strength;
        Networking.LocalPlayer.SetVelocity(playerVel);
        audioSrc.Play();
    }
}
