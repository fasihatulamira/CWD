using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTargetManager : MonoBehaviour
{
    public List<ImageTargetData> imageTargetDataList;
    public GameObject traceImageCanvas; // The canvas where trace images will be displayed

    private Dictionary<string, Image> imageTargetToTraceImageMap;

    void Start()
    {
        imageTargetToTraceImageMap = new Dictionary<string, Image>();

        foreach (var data in imageTargetDataList)
        {
            imageTargetToTraceImageMap[data.imageTargetName] = data.traceImage;
        }
    }

    public void ShowTraceImage(string imageTargetName)
    {
        if (imageTargetToTraceImageMap.ContainsKey(imageTargetName))
        {
            Image traceImage = imageTargetToTraceImageMap[imageTargetName];
            traceImage.gameObject.SetActive(true);
        }
    }

    public void HideTraceImage(string imageTargetName)
    {
        if (imageTargetToTraceImageMap.ContainsKey(imageTargetName))
        {
            Image traceImage = imageTargetToTraceImageMap[imageTargetName];
            traceImage.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class ImageTargetData
{
    public string imageTargetName;
    public Image traceImage;
}