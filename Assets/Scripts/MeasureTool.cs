using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.OpenXR;

using UnityEngine.InputSystem.XR;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;

using TMPro;


public class OpenXRMeasure : MonoBehaviour
{
    ARRaycastManager aRRaycastManager;

    private XRController rightController;

    public GameObject[] measurePoints;
    public GameObject reticle;
    
    public float distancePoints = 0f;

    int currentPoint = 0;

    bool placementEnabled = true;
    
    public TMP_Text distanceText;

    public LineRenderer line;


    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        rightController = GameObject.Find("Right Controller").GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        aRRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon);

        if (hits.Count > 0)
        {
            reticle.transform.position = hits[0].pose.position;
            reticle.transform.position = hits[0].pose.position;

            if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rightSecondaryButtonPressed) && rightSecondaryButtonPressed)
            {
                if (currentPoint < 2)
                {
                    // placing measure point
                    PlacePoints(hits[0].pose.position, currentPoint);
                }
                placementEnabled = false;
                
            }

        }

        if (currentPoint > 1)
        {
            line.enabled = true;
            line.SetPosition(0, measurePoints[0].transform.position);
            line.SetPosition(1, measurePoints[1].transform.position);
        }

        distancePoints = Vector3.Distance(measurePoints[0].transform.position, measurePoints[1].transform.position);

        distanceText.text = distancePoints.ToString();
    }

    public void PlacePoints(Vector3 pointPosition, int pointIndex)
    {
        measurePoints[pointIndex].SetActive(true);
        measurePoints[pointIndex].transform.position = pointPosition;

        currentPoint++;
    }
 




}