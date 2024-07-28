using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.OpenXR;
using UnityEngine.InputSystem;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;
using TMPro;

public class NewMeasureTool : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager aRRaycastManager;

    [SerializeField]
    InputActionReference toggleMeasureTool;
    [SerializeField]
    GameObject pointPrefab;

    [SerializeField]
    Transform rightController;

    [SerializeField]
    TMP_Text distanceText;

    [SerializeField]
    LineRenderer distanceLine; 

    private GameObject point1;
    private GameObject point2;

    private float distancePoints = 0f;

    private bool isFirstPointPlaced = false; 

    private int pointIndex = 0; 
    private bool pointsSet = false; 

    void Start()
    {
        Debug.Log("Measure Tool Active");
        toggleMeasureTool.action.performed += HandleControllerInput; // Subscribe to the input action event
        distanceText.text = "";
    }

    void Update()
    {
        if (isFirstPointPlaced && !pointsSet)
        {
            UpdateLineEnd(); // Update the end point of the distance line
        }
    }

    public void HandleControllerInput(InputAction.CallbackContext context)
    {
        Debug.Log("Points Set");
        SetPoint(); // Set the measurement point
    }

    void SetPoint()
    {
        if (pointIndex < 2)
        {
            Vector3 hitPosition = Vector3.zero;
            if (TryGetHitPosition(out hitPosition))
            {
                PlacePoint(hitPosition); // Place the measurement point at the hit position
                pointIndex++;
                Debug.Log($"Point {pointIndex} set at {hitPosition}");

                if (pointIndex == 2)
                {
                    pointsSet = true;
                    CalculateDistance(); // Calculate the distance between the two points
                }
            }
        }
    }

    bool TryGetHitPosition(out Vector3 position)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        Debug.Log(aRRaycastManager);
        // Raycast from the controller to the detected planes
        if (aRRaycastManager.Raycast(new Ray(rightController.transform.position, rightController.transform.forward), hits, TrackableType.Planes))
        {
            position = hits[0].pose.position;
            Debug.Log($"Hit position: {position}");
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    void PlacePoint(Vector3 position)
    {
        if (!isFirstPointPlaced)
        {
            if (point1 != null)
            {
                Destroy(point1); // Destroy the previous first point
            }
            point1 = Instantiate(pointPrefab, position, Quaternion.identity); // Instantiate the first point prefab
            isFirstPointPlaced = true;
            Debug.Log("First point placed");

            InitializeLine(); // Initialize the distance line
        }
        else
        {
            if (point2 != null)
            {
                Destroy(point2); // Destroy the previous second point
            }
            point2 = Instantiate(pointPrefab, position, Quaternion.identity); // Instantiate the second point prefab
            
            isFirstPointPlaced = false;
            Debug.Log("Second point placed");
        }
    }

    void InitializeLine()
    {
        if (distanceLine == null)
        {
            distanceLine = gameObject.AddComponent<LineRenderer>();
            distanceLine.positionCount = 2; 
            distanceLine.startWidth = 0.05f;
            distanceLine.endWidth = 0.05f; 
            distanceLine.material = new Material(Shader.Find("Sprites/Default")); 
            distanceLine.startColor = Color.red; 
            distanceLine.endColor = Color.red; 
        }

        distanceLine.SetPosition(0, point1.transform.position); // Set the start position of the line renderer
    }

    void UpdateLineEnd()
    {
        Vector3 hitPosition;
        if (TryGetHitPosition(out hitPosition))
        {
            distanceLine.SetPosition(1, hitPosition); // Set the end position of the line renderer
        }
    }

    void CalculateDistance()
    {
        distancePoints = Vector3.Distance(point1.transform.position, point2.transform.position); // Calculate the distance between the two points
        distanceText.text = "Distance: " + (distancePoints * 100).ToString("F2") + " cm"; // Update the distance text
        Debug.Log($"Distance calculated: {distancePoints * 100} cm");
    }
}
