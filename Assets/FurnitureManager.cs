using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]

public class FurnitureManager : MonoBehaviour
{
    //public GameObject[] furnitureItems; // Assign via the inspector

    [SerializeField]
    private GameObject _grabbableCouch;

    private GameObject currentFurniture;

    private ARPlaneManager _planeManager;

    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
    }


    public void SpawnFurniture(int index)
    {
        Vector3 spawnPostion;

        foreach (var plane in _planeManager.trackables)
        {
            if(plane.classification == PlaneClassification.Table)
            {
                spawnPostion = plane.transform.position;
                spawnPostion.y += 0.3f;
                Instantiate(_grabbableCouch, spawnPostion, Quaternion.identity);
            }
        }
      /*   if (currentFurniture != null)
        {
            Destroy(currentFurniture);
        }
        currentFurniture = Instantiate(furnitureItems[index], Vector3.zero, Quaternion.identity);
        var interactable = currentFurniture.AddComponent<XRGrabInteractable>();
        interactable.movementType = XRBaseInteractable.MovementType.Instantaneous; */
    }
}
