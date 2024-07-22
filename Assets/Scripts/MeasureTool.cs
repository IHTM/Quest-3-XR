using UnityEngine;
using UnityEngine.InputSystem;

public class MeasureTool : MonoBehaviour
{
    public GameObject pointPrefab;
    public InputActionReference placePointAction;
    private GameObject point1;
    private GameObject point2;
    private bool isFirstPointPlaced = false;

    private void OnEnable()
    {
        placePointAction.action.performed += OnPlacePoint;
    }

    private void OnDisable()
    {
        placePointAction.action.performed -= OnPlacePoint;
    }

    private void OnPlacePoint(InputAction.CallbackContext context)
    {
        PlacePoint();
    }

    void PlacePoint()
    {
        Vector3 controllerPosition = GetControllerPosition();

        if (!isFirstPointPlaced)
        {
            point1 = Instantiate(pointPrefab, controllerPosition, Quaternion.identity);
            isFirstPointPlaced = true;
        }
        else
        {
            point2 = Instantiate(pointPrefab, controllerPosition, Quaternion.identity);
            CalculateDistance();
            isFirstPointPlaced = false;
        }
    }

    Vector3 GetControllerPosition()
    {
        // Hier erhalten Sie die Position des Controllers direkt aus dem Input-System.
        // Dies ist ein Platzhalter, da die genaue Methode davon abhängt, wie Ihr XR-Rig konfiguriert ist.
        // Zum Beispiel könnte dies eine Kamera sein, die die Position des Controllers verfolgt.
        // Hier ein Beispiel, das möglicherweise angepasst werden muss:

        Transform cameraTransform = Camera.main.transform;
        return cameraTransform.position + cameraTransform.forward * 0.5f; // Platzhalter-Position vor der Kamera
    }

    void CalculateDistance()
    {
        if (point1 != null && point2 != null)
        {
            float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
            Debug.Log("Abstand zwischen den Punkten: " + distance + " Meter");
            // Optional: Zeigen Sie den Abstand im UI an
        }
    }
}
