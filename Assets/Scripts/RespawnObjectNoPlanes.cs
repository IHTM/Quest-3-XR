using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class NewBehaviourScript : MonoBehaviour
{
 public GameObject objectPrefab;
    public ARPlaneManager planeManager;
    public Camera playerCamera;
    public float spawnDistance = 2.0f;  // Distance in front of the camera to spawn the object

    void Start()
    {
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
        }
    }

    // Method to be called by the UI button
    public void OnRespawnButtonClick()
    {
        Vector3 spawnPoint = playerCamera.transform.position + playerCamera.transform.forward * spawnDistance;
        
        // Check if the spawnPoint is not on any existing planes
        if (!IsPointOnPlane(spawnPoint))
        {
            Instantiate(objectPrefab, spawnPoint, Quaternion.identity);
            Debug.Log("Object respawned successfully!");
        }
        else
        {
            Debug.LogError("Spawn point is on an existing plane. Try a different location.");
        }
    }

    private bool IsPointOnPlane(Vector3 point)
    {
        foreach (var plane in planeManager.trackables)
        {
            if (plane.extents.magnitude == 0)
                continue;  // Skip planes that are not yet properly initialized

            // Check if the point is within the bounds of the plane
            if (plane.boundary.Contains(plane.transform.InverseTransformPoint(point)))
                return true;
        }
        return false;
    } 
}
    
