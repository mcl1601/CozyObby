
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WorldToggles : UdonSharpBehaviour
{
    [SerializeField] GameObject colliders, audioLink;
    [SerializeField] GameObject[] crowns;
    void Start()
    {
        
    }

    public void ToggleColliders()
    {
        colliders.SetActive(!colliders.activeSelf);
    }

    public void ToggleAL()
    {
        audioLink.SetActive(!audioLink.activeSelf);
    }

    public void ToggleCrowns()
    {
        foreach(GameObject g in crowns)
        {
            g.SetActive(!g.activeSelf);
        }
    }
}
