
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MuffleSoundManager : UdonSharpBehaviour
{
    public bool isOn, isOn2, actuallyOn;
    [SerializeField] private AudioSource[] audioSources, muffledAudioSources;
    
    void Start()
    {
        
    }

    public void DoNormalAudio()
    {
        for(int i = 0; i < audioSources.Length; i++)
        {
            int timeStamp = muffledAudioSources[i].timeSamples;
            audioSources[i].gameObject.SetActive(!audioSources[i].gameObject.activeSelf);
            muffledAudioSources[i].gameObject.SetActive(!muffledAudioSources[i].gameObject.activeSelf);
            audioSources[i].timeSamples = timeStamp;
            audioSources[i].Play();
        }
    }

    public void DoMuffledAudio()
    {
        for(int i = 0; i < audioSources.Length; i++)
        {
            int timeStamp = audioSources[i].timeSamples;
            audioSources[i].gameObject.SetActive(!audioSources[i].gameObject.activeSelf);
            muffledAudioSources[i].gameObject.SetActive(!muffledAudioSources[i].gameObject.activeSelf);
            muffledAudioSources[i].timeSamples = timeStamp;
            muffledAudioSources[i].Play();
        }
    }

    public void CheckValues()
    {
        if(!actuallyOn && (isOn || isOn2))
        {
            actuallyOn = true;
            DoMuffledAudio();
        }
        else if(actuallyOn && (!isOn && !isOn2))
        {
            actuallyOn = false;
            DoNormalAudio();
        }
    }
}
