using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class NewMeasureTool : MonoBehaviour
{
    public GameObject pointPrefab;  // Prefab für die Punkte im Raum
    public GameObject distanceTextPrefab; // Prefab für den Text zur Distanzanzeige

    // public XRRayInteractor interactor;
    public XRController xrController;

    private GameObject point1;
    private GameObject point2;
    private GameObject distanceText;

    private bool isFirstPointSet = false;


    void Start()
    {
        // Finde den XRRayInteractor automatisch in der Szene
       /*  interactor = FindObjectOfType<ARRaycastManager>();

        if (aRRaycastManager == null)
        {
            Debug.LogError("ARRaycastManager not found in the scene. Please make sure it is added to the AR Session Origin.");
        } */

        // Finde den XRController automatisch in der Szene
        xrController = FindObjectOfType<XRController>();

        if (xrController == null)
        {
            Debug.LogError("XRController not found in the scene. Please make sure it is added to the controller.");
        }
    }


    void Update()
    {
        if (xrController != null)
        {
            // bool buttonPressed = false;
            if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool buttonPressed) && buttonPressed)
            {
                if (!isFirstPointSet)
                {
                    point1 = Instantiate(pointPrefab, xrController.transform.position, Quaternion.identity);
                    isFirstPointSet = true;
                }
                else
                {
                    point2 = Instantiate(pointPrefab, xrController.transform.position, Quaternion.identity);
                    DisplayDistance();
                    isFirstPointSet = false;  // Zurücksetzen für die nächste Messung
                }
            }
            
            
            /* {
                if (!isFirstPointSet)
                {
                    point1 = CreatePointAtRaycastHit();
                    if (point1 != null)
                    {
                        isFirstPointSet = true;
                        Debug.Log("First point set at: " + point1.transform.position);
                    }
                }
                else
                {
                    point2 = CreatePointAtRaycastHit();
                    if (point2 != null)
                    {
                        DisplayDistance();
                        isFirstPointSet = false;  // Zurücksetzen für die nächste Messung
                        Debug.Log("Second point set at: " + point2.transform.position);
                    }
                }
            } */
        }
    }

/*     private GameObject CreatePointAtRaycastHit()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;
            return Instantiate(pointPrefab, hitPose.position, Quaternion.identity);
        }

        return null;
    } */

    private void DisplayDistance()
    {
        if (point1 != null && point2 != null)
        {
            float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
            Debug.Log("Distance: " + distance + " meters");

            // Anzeigen der Distanz als Text im Raum
            if (distanceText == null)
            {
                distanceText = Instantiate(distanceTextPrefab, (point1.transform.position + point2.transform.position) / 2, Quaternion.identity);
            }

            distanceText.GetComponent<TextMesh>().text = distance.ToString("F2") + " meters";
            distanceText.transform.position = (point1.transform.position + point2.transform.position) / 2;
        }
    }
}
