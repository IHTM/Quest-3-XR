using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR;
using TMPro;

public class OpenXRMeasure : MonoBehaviour
{
    public GameObject measurePointPrefab;
    public TextMeshProUGUI distanceText;
    private List<GameObject> measurePoints = new List<GameObject>();
    private LineRenderer lineRenderer;
    private bool isMeasuring = false;
    private Camera arCamera;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        arCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Ray ray = arCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject measurePoint = Instantiate(measurePointPrefab, hit.point, Quaternion.identity);
                measurePoints.Add(measurePoint);

                if (measurePoints.Count == 2)
                {
                    isMeasuring = true;
                    UpdateMeasurement();
                }
            }
        }

        if (isMeasuring)
        {
            UpdateMeasurement();
        }
    }

    void UpdateMeasurement()
    {
        if (measurePoints.Count == 2)
        {
            Vector3 point1 = measurePoints[0].transform.position;
            Vector3 point2 = measurePoints[1].transform.position;
            float distance = Vector3.Distance(point1, point2);

            distanceText.text = $"Distance: {distance:F2} meters";

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, point1);
            lineRenderer.SetPosition(1, point2);
        }
    }

    public void ResetMeasurement()
    {
        foreach (var point in measurePoints)
        {
            Destroy(point);
        }
        measurePoints.Clear();
        lineRenderer.positionCount = 0;
        distanceText.text = "Distance: 0 meters";
        isMeasuring = false;
    }
}
