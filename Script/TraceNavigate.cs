using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TraceNavigate : MonoBehaviour
{
    public GameObject image;
    public GameObject buttonBack;
    public GameObject ARTrace;
    public GameObject Script;
    public BolaPlay bolaPlay; // Reference to the BolaPlay script
    public GameObject buttonDelete;
    public GameObject videoOverlay; // Reference to the video overlay GameObject

    private ARDrawOnPlane arDrawOnPlane;

    void Start()
    {
        // Get the ARDrawOnPlane component from the Script GameObject
        if (Script != null)
        {
            arDrawOnPlane = Script.GetComponent<ARDrawOnPlane>();
        }
    }

    public void Trace()
    {
        // Stop audio and video if they are playing
        if (bolaPlay != null)
        {
            bolaPlay.StopAudio();
            bolaPlay.StopVideo();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.MuteAllImageTargetAudio(); // Mute all image target's audio sources
                Debug.Log("Called MuteAllImageTargetAudio from Trace");
            }
        }

        // Disable the video overlay
        if (videoOverlay != null)
        {
            videoOverlay.SetActive(false);
        }

        //SceneManager.LoadScene("TestDraw");
        if (image != null) image.SetActive(true);
        if (buttonBack != null) buttonBack.SetActive(true);
        if (ARTrace != null) ARTrace.SetActive(true);
        if (Script != null) Script.GetComponent<ARDrawOnPlane>().enabled = true;
        if (buttonDelete != null) buttonDelete.SetActive(true);
    }

    public void TraceOff()
    {
        if (image != null) image.SetActive(false);
        if (buttonBack != null) buttonBack.SetActive(false);
        if (ARTrace != null) ARTrace.SetActive(false);
        if (Script != null) Script.GetComponent<ARDrawOnPlane>().enabled = false;
        if (buttonDelete != null) buttonDelete.SetActive(false);

        // Re-enable the video overlay
        if (videoOverlay != null)
        {
            videoOverlay.SetActive(true);
        }

        if (bolaPlay != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.UnmuteAllImageTargetAudio(); // Unmute all image target's audio sources
            Debug.Log("Called UnmuteAllImageTargetAudio from TraceOff");
        }
    }

    public void ClearDrawing()
    {
        // Call the ClearDrawing method from the ARDrawOnPlane script
        if (arDrawOnPlane != null)
        {
            arDrawOnPlane.ClearDrawing();
        }
    }
}