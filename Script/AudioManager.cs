using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<AudioSource> imageTargetAudioSources = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterAudioSource(AudioSource audioSource)
    {
        if (!imageTargetAudioSources.Contains(audioSource))
        {
            imageTargetAudioSources.Add(audioSource);
            Debug.Log("Registered AudioSource: " + audioSource.gameObject.name);
        }
    }

    public void MuteAllImageTargetAudio()
    {
        Debug.Log("Muting all image target audio sources.");
        foreach (var audioSource in imageTargetAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.mute = true;
                Debug.Log("Muted AudioSource: " + audioSource.gameObject.name);
            }
        }
    }

    public void UnmuteAllImageTargetAudio()
    {
        Debug.Log("Unmuting all image target audio sources.");
        foreach (var audioSource in imageTargetAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.mute = false;
                Debug.Log("Unmuted AudioSource: " + audioSource.gameObject.name);
            }
        }
    }
}