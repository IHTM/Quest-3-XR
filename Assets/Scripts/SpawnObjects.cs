using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnObjects : MonoBehaviour
{
    public GameObject[] furniturePrefabs; // Assign prefabs in Unity Inspector
    private ARRaycastManager raycastManager;
    private Camera arCamera;

    void Start()
    {
        // Get the ARRaycastManager from the AR Session Origin
        raycastManager = FindObjectOfType<ARRaycastManager>();
        arCamera = Camera.main;
    }

    // Method to be called by UI buttons
    public void SpawnFurniture(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= furniturePrefabs.Length)
        {
            Debug.LogError("Prefab index out of range.");
            return;
        }

        // Raycast from the center of the screen forward
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            // Get the closest hit point
            Pose hitPose = hits[0].pose;
            Instantiate(furniturePrefabs[prefabIndex], hitPose.position, hitPose.rotation);
        }
        else
        {
            Debug.Log("No plane found at screen center.");
        }
    }
}
