using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARDrawOnPlane : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public GameObject linePrefab;

    private LineRenderer currentLineRenderer;
    private List<Vector3> points = new List<Vector3>();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            // On touch start, create a new line
            if (touch.phase == TouchPhase.Began)
            {
                CreateLine();
            }

            // While moving or holding finger, update the line on the AR plane
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                UpdateLine(touchPosition);
            }
        }
    }

    void CreateLine()
    {
        GameObject newLine = Instantiate(linePrefab);
        currentLineRenderer = newLine.GetComponent<LineRenderer>();
        points.Clear();
    }

    void UpdateLine(Vector2 touchPosition)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        // Raycast from the touch point to an AR plane
        if (raycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Add point if far enough from the last one
            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], hitPose.position) > 0.01f)
            {
                points.Add(hitPose.position);
                currentLineRenderer.positionCount = points.Count;
                currentLineRenderer.SetPositions(points.ToArray());
            }
        }
    }
}