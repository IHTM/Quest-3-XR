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
        toggleMeasureTool.action.performed += HandleControllerInput;
        distanceText.text = "";
    }

    void Update()
    {
        if (pointsSet)
        {
            DrawLine();
        }
    }

    public void HandleControllerInput(InputAction.CallbackContext context)
    {
        Debug.Log("Points Set");
        SetPoint();
    }

    [ContextMenu("Set Point")]
    void SetPoint()
    {
        if (pointIndex < 2)
        {
            Vector3 hitPosition = Vector3.zero;
            if (TryGetHitPosition(out hitPosition))
            {
                PlacePoint(hitPosition);
                pointIndex++;
                Debug.Log($"Point {pointIndex} set at {hitPosition}");

                if (pointIndex == 2)
                {
                    pointsSet = true;
                }
            }
        }
    }

    bool TryGetHitPosition(out Vector3 position)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        Debug.Log(aRRaycastManager);
        if (aRRaycastManager.Raycast(new Ray(rightController.transform.position, rightController.transform.forward), hits, TrackableType.Planes))
        {
            position = hits[0].pose.position;
            Debug.Log($"Hit position: {position}");
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    [ContextMenu("Place Point")]
    void PlacePoint(Vector3 position)
    {
        if (!isFirstPointPlaced)
        {
            if (point1 != null)
            {
                Destroy(point1);
            }
            point1 = Instantiate(pointPrefab, position, Quaternion.identity);
            isFirstPointPlaced = true;
            Debug.Log("First point placed");

        }
        else
        {
            if (point2 != null)
            {
                Destroy(point2);
            }
            point2 = Instantiate(pointPrefab, position, Quaternion.identity);

            isFirstPointPlaced = false;
            Debug.Log("Second point placed");
        }

        if (point1 != null && point2 != null)
        {
            CalculateDistance();
        }
    }

    void CalculateDistance()
    {
        distancePoints = Vector3.Distance(point1.transform.position, point2.transform.position);
        distanceText.text = distancePoints.ToString();
        Debug.Log($"Distance calculated: {distancePoints * 100} cm");
    }

    [ContextMenu("Draw Line")]
    void DrawLine()
    {
        if (point1 != null && point2 != null)
        {
            Debug.Log("Drawing line between points");

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

            distanceLine.SetPosition(0, point1.transform.position);
            distanceLine.SetPosition(1, point2.transform.position);
        }
        else
        {
            Debug.Log("Points are not set correctly");
        }
    }
}
