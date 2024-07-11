using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Capture: MonoBehaviour
{
 public Camera cameraToUse;
    public RenderTexture renderTexture;
    public GameObject objectToCapture;
    public float rotationAngleY = 45.0f;  
    public float rotationAngleX = 45.0f;  
    public float rotationAngleZ = 45.0f;  



    void Start()
    {
        PositionCamera();
        RotateObject();
        StartCoroutine(CaptureScreenshotTransparent());
    }

    void PositionCamera()
    {
        // Calculate the center and the best distance based on the object bounds
        Bounds bounds = objectToCapture.GetComponent<Renderer>().bounds;
        Vector3 objectCenter = bounds.center;
        float cameraDistance = bounds.extents.magnitude * 3.5f; // Adjust this factor as needed

        // Adjust camera position
        cameraToUse.transform.position = objectCenter - cameraToUse.transform.forward * cameraDistance;
        cameraToUse.transform.LookAt(objectCenter);

        // Set orthographic size if camera is orthographic
        if (cameraToUse.orthographic)
        {
            cameraToUse.orthographicSize = bounds.extents.magnitude;
        }
    }

    void RotateObject()
    {
        objectToCapture.transform.Rotate(Vector3.up, rotationAngleY);
        objectToCapture.transform.Rotate(Vector3.right, rotationAngleX);
        objectToCapture.transform.Rotate(Vector3.forward, rotationAngleZ);
    }

    IEnumerator CaptureScreenshotTransparent()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture.active = cameraToUse.targetTexture;

        Texture2D image = new Texture2D(cameraToUse.targetTexture.width, cameraToUse.targetTexture.height, TextureFormat.ARGB32, false);
        image.ReadPixels(new Rect(0, 0, cameraToUse.targetTexture.width, cameraToUse.targetTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = null;

        byte[] bytes = image.EncodeToPNG();
        Destroy(image);

        File.WriteAllBytes("Assets/Screenshots/box2.png", bytes);
    }
}
