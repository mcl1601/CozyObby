
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CollectibleManager : UdonSharpBehaviour
{
    [SerializeField] int numCollectibles;
    [SerializeField] AudioClip collectedSound;
    [SerializeField] GameObject[] prizeObjects;
    private int currentCount = 0;
    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void Collected()
    {
        currentCount++;
        PlayCollectedSound();

        if(currentCount == numCollectibles)
        {
            foreach(GameObject g in prizeObjects)
            {
                g.SetActive(true);
            }
        }
    }

    private void PlayCollectedSound()
    {
        audioSrc.PlayOneShot(collectedSound);
    }
}
