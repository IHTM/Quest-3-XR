using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using TMPro;

public class LineManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager aRRaycastManager;
    [SerializeField] private Transform rightController;
    [SerializeField] private LineRenderer lineRendererPrefab;
    [SerializeField] private InputActionReference toggleMeasureTool;
    [SerializeField] private TMP_Text distanceTextPrefab;

    private LineRenderer currentLine;
    public bool continuous;
    private int pointCount = 0;

    private void Start()
    {
        toggleMeasureTool.action.performed += HandleControllerInput;
    }

    private void OnDestroy()
    {
        toggleMeasureTool.action.performed -= HandleControllerInput;
    }

    private void HandleControllerInput(InputAction.CallbackContext context)
    {
        if (TryGetHitPosition(out Vector3 hitPosition, out Vector3 hitNormal))
        {
            DrawLine(hitPosition, hitNormal);
        }
    }

    private bool TryGetHitPosition(out Vector3 position, out Vector3 normal)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (aRRaycastManager.Raycast(new Ray(rightController.position, rightController.forward), hits, TrackableType.PlaneWithinPolygon))
        {
            position = hits[0].pose.position;
            normal = hits[0].pose.rotation * Vector3.up;
            return true;
        }

        position = Vector3.zero;
        normal = Vector3.up;
        return false;
    }

    
    [ContextMenu("DrawLine")]
    private void DrawLine(Vector3 position, Vector3 normal)
    {
        if (currentLine == null || !continuous)
        {
            currentLine = Instantiate(lineRendererPrefab);
            currentLine.positionCount = 0;
            pointCount = 0;
        }

        currentLine.positionCount = pointCount + 1;
        currentLine.SetPosition(pointCount, position);
        pointCount++;

        if (pointCount > 1)
        {
            Vector3 point1 = currentLine.GetPosition(pointCount - 1);
            Vector3 point2 = currentLine.GetPosition(pointCount - 2);
            float dist = Vector3.Distance(point1, point2);

            TMP_Text distanceText = Instantiate(distanceTextPrefab);
            distanceText.text = $"{dist:F2} meters";

            Vector3 directionVector = (point1 - point2);
            Vector3 upd = Vector3.Cross(directionVector, normal).normalized;
            Quaternion rotation = Quaternion.LookRotation(-normal, upd);

            distanceText.transform.rotation = rotation;
            distanceText.transform.position = (point1 + point2) * 0.5f + upd * 0.2f;
        }
    }
}
