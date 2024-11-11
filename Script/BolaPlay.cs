using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class BolaPlay : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public AudioSource audioSource; // Reference to the AudioSource component
    public List<AudioSource> imageTargetAudioSources; // List of AudioSource components for all image targets
    public Image videoOverlay; // Reference to the overlay Image component
    public Button audioButton; // Reference to the audio button
    public ARSession arSession; // Reference to the ARSession component
    public ARTrackedImageManager arTrackedImageManager; // Reference to the ARTrackedImageManager component

    private bool isVideoPlaying = false;
    private bool isAudioPlaying = false;
    private bool isVideoPrepared = false;
    private bool isImageTargetFound = false; // Flag to track if any image target is found

    void Start()
    {
        // Ensure the video and audio are not playing on start
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned.");
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
        else
        {
            Debug.LogError("AudioSource is not assigned.");
        }

        if (imageTargetAudioSources != null)
        {
            foreach (var audioSource in imageTargetAudioSources)
            {
                audioSource.Stop();
            }
        }
        else
        {
            Debug.LogError("ImageTargetAudioSources list is not assigned.");
        }

        // Register the image target's audio sources with the AudioManager
        if (AudioManager.Instance != null && imageTargetAudioSources != null)
        {
            foreach (var audioSource in imageTargetAudioSources)
            {
                AudioManager.Instance.RegisterAudioSource(audioSource);
                Debug.Log("Registered image target audio source: " + audioSource.gameObject.name);
            }
        }

        // Add a click event listener to the overlay image
        if (videoOverlay != null)
        {
            Button overlayButton = videoOverlay.gameObject.AddComponent<Button>();
            overlayButton.onClick.AddListener(ToggleVideoPlayback);
        }
        else
        {
            Debug.LogError("VideoOverlay is not assigned.");
        }

        // Add a click event listener to the audio button
        if (audioButton != null)
        {
            audioButton.onClick.AddListener(ToggleAudio);
        }
        else
        {
            Debug.LogError("AudioButton is not assigned.");
        }

        // Ensure audio and video are stopped if the image target is not found
        SetImageTargetFound(false);

        // Check if ARSession is not null and log its state
        if (ARSession.state != ARSessionState.None)
        {
            Debug.Log("ARSession state: " + ARSession.state);
        }
        else
        {
            Debug.LogError("ARSession is not initialized. Ensure it is set in the inspector.");
        }

        // Check if ARTrackedImageManager is not null and log its state
        if (arTrackedImageManager != null)
        {
            arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            Debug.Log("ARTrackedImageManager is initialized.");
        }
        else
        {
            Debug.LogError("ARTrackedImageManager is null. Ensure it is set in the inspector.");
        }
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        isVideoPrepared = true;
        Debug.Log("VideoPlayer is prepared.");
    }

    // Called when the "Video Play" button is clicked
    public void PlayVideo()
    {
        if (videoPlayer != null && isVideoPrepared)
        {
            // Stop the audio when video starts playing
            if (audioSource != null && isAudioPlaying)
            {
                audioSource.Stop();
                isAudioPlaying = false;
            }

            if (isVideoPlaying)
            {
                videoPlayer.Stop();
                isVideoPlaying = false;
                Debug.Log("Video stopped.");
            }
            else
            {
                videoPlayer.Play();
                isVideoPlaying = true;
                Debug.Log("Video started.");
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.MuteAllImageTargetAudio();
                Debug.Log("Called MuteAllImageTargetAudio from PlayVideo");
            }

            // Stop all image target audio sources when video starts playing
            if (imageTargetAudioSources != null)
            {
                foreach (var audioSource in imageTargetAudioSources)
                {
                    audioSource.Stop();
                }
            }
        }
        else
        {
            Debug.LogWarning("VideoPlayer is not prepared or reference is null.");
        }
    }

    // Toggle the audio playback state (start/stop)
    public void ToggleAudio()
    {
        if (audioSource != null)
        {
            // Stop the video when audio starts playing
            if (videoPlayer != null && isVideoPlaying)
            {
                videoPlayer.Stop();
                isVideoPlaying = false;
            }

            if (isAudioPlaying)
            {
                audioSource.Stop();
                isAudioPlaying = false;
                Debug.Log("Audio stopped.");
            }
            else
            {
                audioSource.Play();
                isAudioPlaying = true;
                Debug.Log("Audio started.");
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.MuteAllImageTargetAudio();
                Debug.Log("Called MuteAllImageTargetAudio from ToggleAudio");
            }

            // Stop all image target audio sources when audio starts playing
            if (imageTargetAudioSources != null)
            {
                foreach (var audioSource in imageTargetAudioSources)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    // Stop the video
    public void StopVideo()
    {
        if (videoPlayer != null && isVideoPlaying)
        {
            videoPlayer.Stop();
            isVideoPlaying = false;
            Debug.Log("Video stopped.");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.UnmuteAllImageTargetAudio();
                Debug.Log("Called UnmuteAllImageTargetAudio from StopVideo");
            }

            // Resume all image target audio sources when video stops
            if (imageTargetAudioSources != null && isImageTargetFound)
            {
                foreach (var audioSource in imageTargetAudioSources)
                {
                    audioSource.Play();
                }
            }
        }
    }

    // Stop the audio
    public void StopAudio()
    {
        if (audioSource != null && isAudioPlaying)
        {
            audioSource.Stop();
            isAudioPlaying = false;
            Debug.Log("Audio stopped.");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.UnmuteAllImageTargetAudio();
                Debug.Log("Called UnmuteAllImageTargetAudio from StopAudio");
            }

            // Resume all image target audio sources when audio stops
            if (imageTargetAudioSources != null && isImageTargetFound)
            {
                foreach (var audioSource in imageTargetAudioSources)
                {
                    audioSource.Play();
                }
            }
        }
    }

    // Toggle the video playback state (pause/resume)
    public void ToggleVideoPlayback()
    {
        if (videoPlayer != null && isVideoPrepared)
        {
            if (isVideoPlaying)
            {
                videoPlayer.Pause();
                isVideoPlaying = false;
                Debug.Log("Video paused. Current frame: " + videoPlayer.frame);
            }
            else
            {
                videoPlayer.Play();
                isVideoPlaying = true;
                Debug.Log("Video resumed. Current frame: " + videoPlayer.frame);
            }
        }
        else
        {
            Debug.LogWarning("VideoPlayer is not prepared or reference is null.");
        }
    }

    public void SetAudio(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    public void SetVideo(VideoPlayer videoPlayer)
    {
        this.videoPlayer = videoPlayer;
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Function to handle "trace button" click
    public void GoToTestDraw()
    {
        //SceneManager.LoadScene("TestDraw");
    }

    // Method to set the image target found state
    public void SetImageTargetFound(bool found)
    {
        isImageTargetFound = found;
        if (!found)
        {
            StopVideo();
            StopAudio();

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.UnmuteAllImageTargetAudio();
                Debug.Log("Called UnmuteAllImageTargetAudio from SetImageTargetFound");
            }
        }
    }

    // Method to check if the image target is found
    public bool IsImageTargetFound()
    {
        return isImageTargetFound;
    }

    // Event handler for tracked images changed
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log("Image target detected: " + trackedImage.referenceImage.name);
            SetImageTargetFound(true);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            Debug.Log("Image target updated: " + trackedImage.referenceImage.name);
            SetImageTargetFound(true);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log("Image target removed: " + trackedImage.referenceImage.name);
            SetImageTargetFound(false);
        }
    }
}