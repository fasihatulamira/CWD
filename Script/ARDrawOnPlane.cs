using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI; // Add this to use UI components

public class ARDrawOnPlane : MonoBehaviour
{
    public ARRaycastManager raycastManager; // Manages raycasting in AR
    public ARPlaneManager planeManager; // Manages AR planes
    public GameObject linePrefab; // Prefab for the line to be drawn
    public Button deleteButton; // Button to clear the drawing

    private LineRenderer currentLineRenderer; // Reference to the current line being drawn
    private List<Vector3> points = new List<Vector3>(); // List to store points of the current line

    void Start()
    {
        // Link the Delete button to the ClearDrawing method
        deleteButton.onClick.AddListener(ClearDrawing);
    }

    void Update()
    {
        // Check if there is at least one touch input
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
        // Instantiate a new line from the prefab
        GameObject newLine = Instantiate(linePrefab);
        // Get the LineRenderer component from the new line
        currentLineRenderer = newLine.GetComponent<LineRenderer>();
        // Clear the points list for the new line
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

    public void ClearDrawing()
    {
        // Destroy the current line and clear the points list
        if (currentLineRenderer != null)
        {
            Destroy(currentLineRenderer.gameObject);
            points.Clear();
        }
    }
}