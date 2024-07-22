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
    public ARRaycastManager aRRaycastManager;

    public InputActionReference toggleMeasureTool;

    public float distancePoints = 0f;

    public GameObject pointPrefab;

    private GameObject point1;
    private GameObject point2;

    [SerializeField]
    Transform rightController;
    private bool isFirstPointPlaced = false;

    private int pointIndex = 0;
    private bool pointsSet = false;

    public TMP_Text distanceText;
    public LineRenderer line;

    void Start()
    {
        // input WIP

        Debug.Log("Measure Tool Active");
        toggleMeasureTool.action.performed += HandleControllerInput;
        line.positionCount = 2;
        distanceText.text = "";
    }

    void Update()
    {
        
/*         if (pointsSet)
        {

        } */
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
                Destroy(point1);
            }
            point1 = Instantiate(pointPrefab, position, Quaternion.identity);
            isFirstPointPlaced = true;
        }
        else
        {
            if (point2 != null)
            {
                Destroy(point2);
            }
            point2 = Instantiate(pointPrefab, position, Quaternion.identity);
            
            isFirstPointPlaced = false;
        }
        if (point1 != null && point2 != null)
        {
            CalculateDistance();
        }
    }

    void CalculateDistance()
    {
        distancePoints = Vector3.Distance(point1.transform.position, point2.transform.position);
        distanceText.text = "Distance: " + distancePoints.ToString("F2") + "m";
    }


}
