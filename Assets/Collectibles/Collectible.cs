
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class Collectible : UdonSharpBehaviour
{
    private CollectibleManager manager;
    void Start()
    {
        manager = transform.parent.GetComponent<CollectibleManager>();
    }

    public override void Interact()
    {
        manager.Collected();

        gameObject.SetActive(false);
    }
}
