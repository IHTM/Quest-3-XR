using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;



[RequireComponent(typeof(ARPlaneManager))]
public class MeasurementSceneController : MonoBehaviour
{

    [SerializeField]
    private InputActionReference _toggleMeasureTool;

    private ARPlaneManager _arPlaneManager;
    private bool _isVisible = true;
    private int _numPlanesAddedOccurred = 0;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MeasurementController Start");
        _arPlaneManager = GetComponent<ARPlaneManager>();

        if(_arPlaneManager == null)
        {
            Debug.LogError("ARPlaneManager is not found : ( ");
        }

        _toggleMeasureTool.action.performed += OnToggleMeasureToolAction;
        _arPlaneManager.planesChanged += OnPlanesChanged;
    }

    // Update is called once per frame
    void Update()
    {
 /*        if (activated && Input.touchCount > 0)
        {
            Debug.Log("Touch detected");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch began");
                touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    Debug.Log("within plane");
                    startPoint.SetActive(true);

                    Pose hitPose = hits[0].pose;
                    startPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Debug.Log("Touch moved");
                touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    Debug.Log("Within plane2");
                    measureLine.gameObject.SetActive(true);
                    endPoint.SetActive(true);

                    Pose hitPose = hits[0].pose;
                    endPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }
        }

        if (startPoint.activeSelf && endPoint.activeSelf)
        {
            Debug.Log("Try calc distance");
            distanceText.transform.position = endPoint.transform.position + offsetMeasurement;
            distanceText.transform.rotation = endPoint.transform.rotation;
            measureLine.SetPosition(0, startPoint.transform.position);
            measureLine.SetPosition(1, endPoint.transform.position);

            distanceText.text = $"Distance: {(Vector3.Distance(startPoint.transform.position, endPoint.transform.position) * measurementFactor * 100).ToString("F2")} cm";
        } */
    }

    public void OnToggleMeasureToolAction(InputAction.CallbackContext obj)
    {
        _isVisible = !_isVisible;
        float fillAlpha = _isVisible ? 0.3f : 0.0f;
        float lineAlpha = _isVisible ? 0.9f : 0.0f;

        foreach (var plane in _arPlaneManager.trackables)
        {
            SetPlaneAlpha(plane, fillAlpha, lineAlpha);
        }

    }


    public void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
    {
        if (eventArgs.added.Count > 0)
        {
            _numPlanesAddedOccurred++;

            foreach(var plane in _arPlaneManager.trackables)
            {
                PrintPlaneLabel(plane);
            }

            Debug.Log($"Number of planes: " + _arPlaneManager.trackables.count);
            Debug.Log($"Planes added: {_numPlanesAddedOccurred}");
        }
    }

    private void PrintPlaneLabel(ARPlane plane)
    {
        string label = plane.classification.ToString();
        string log = $"Plane {plane.trackableId} with label {label}";
        Debug.Log(log);
    }

    private void SetPlaneAlpha(ARPlane plane, float fillAlpha, float lineAlpha)
    {
        var meshRenderer = plane.GetComponentInChildren<MeshRenderer>();
        var lineRenderer = plane.GetComponentInChildren<LineRenderer>();

        if(meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = fillAlpha;
            meshRenderer.material.color = color;
        }

        if(lineRenderer != null)
        {
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;

            startColor.a = lineAlpha;
            endColor.a = lineAlpha;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }

    void OnDestroy()
    {
        _toggleMeasureTool.action.performed -= OnToggleMeasureToolAction;
        _arPlaneManager.planesChanged -= OnPlanesChanged;
    }

}

