
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Checkpoint : UdonSharpBehaviour
{
    public Transform tran;
    private ObbyManager manager;
    public MeshRenderer mat;
    [SerializeField] bool isStart = false;
    void Start()
    {
        tran = transform;
        manager = transform.parent.GetComponent<ObbyManager>();
        mat = GetComponent<MeshRenderer>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if(player != Networking.LocalPlayer) return;

        if(isStart)
        {
            manager.StartCPHit(this);
            return;
        }

        manager.SetActiveCheckpoint(this);
    }
}
