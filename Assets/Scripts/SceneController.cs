using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class SceneController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _togglePlanesAction;

    [SerializeField]
    private InputActionReference _activateAction;

    [SerializeField]
    private GameObject _grabbableCouch;
    
    [SerializeField]
    private GameObject _canvas;

    private ARPlaneManager _planeManager;
    private bool _isVisibible = true;

    // Start is called before the first frame update
    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();

        _togglePlanesAction.action.performed += OnTogglePlanesAction;
        // _activateAction.action.performed += OnActivateAction;
    }

/*     private void OnActivateAction(InputAction.CallbackContext context)
    {
        SpawnGrabbableCouch();
    } */

/*     private void SpawnGrabbableCouch()
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

    } */

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        throw new NotImplementedException();
    }

    private void OnTogglePlanesAction(InputAction.CallbackContext context)
    {
/*         _isVisibible = !_isVisibible;
        float fillAlpha = _isVisibible ? 0.3f : 0f;
        float lineAlpha = _isVisibible ? 1.0f : 0f; */

        _canvas.SetActive(!_canvas.activeSelf);
 

  /*       foreach (var plane in _planeManager.trackables)
        {
            SetPlaneAlpha(plane, fillAlpha, lineAlpha);
        } */
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        _togglePlanesAction.action.performed -= OnTogglePlanesAction;
        // _activateAction.action.performed -= OnActivateAction;
    }
}
