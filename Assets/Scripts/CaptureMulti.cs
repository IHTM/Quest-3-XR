using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class CaptureMulti : MonoBehaviour
{
    public Camera cameraToUse;
    public RenderTexture renderTexture;
    public GameObject[] prefabsToCapture; // Array of prefabs to capture
    public Vector3 cameraPositionOffset = new Vector3(0, 0, -5); // Camera offset for a good view
    public string saveFolderPath = "Assets/Screenshots/"; // Ensure this folder exists in your Unity Assets
    public Vector3 rotationAngles; // Input fields for specifying rotation angles for the prefab

    void Start()
    {
        StartCoroutine(CapturePrefabs());
    }

    IEnumerator CapturePrefabs()
    {
        foreach (GameObject prefab in prefabsToCapture)
        {
            GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity); // Instantiate prefab at the origin
            RotatePrefab(instance, rotationAngles);
            PositionCamera(instance); // Position the camera using the offset
            yield return new WaitForEndOfFrame(); // Wait for the frame to ensure the scene is rendered
            CaptureAndSavePrefab(instance.name);
            Destroy(instance); // Destroy the instance to clean up the scene
            yield return null;
        }
    }

    void RotatePrefab(GameObject prefab, Vector3 angles)
    {
        // Apply rotation based on the specified input fields
        prefab.transform.eulerAngles = angles;
    }

    void PositionCamera(GameObject target)
    {
        // Use the offset to position the camera relative to the prefab
        cameraToUse.transform.position = target.transform.position + cameraPositionOffset;
        cameraToUse.transform.LookAt(target.transform.position);
    }

    void CaptureAndSavePrefab(string prefabName)
    {
        RenderTexture.active = cameraToUse.targetTexture;

        Texture2D image = new Texture2D(cameraToUse.targetTexture.width, cameraToUse.targetTexture.height, TextureFormat.ARGB32, false);
        image.ReadPixels(new Rect(0, 0, cameraToUse.targetTexture.width, cameraToUse.targetTexture.height), 0, 0);
        image.Apply();

        // Encode texture into PNG
        byte[] bytes = image.EncodeToPNG();
        Destroy(image); // Clean up the texture from memory

        // Ensure directory exists
        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);

        // Save the image to a file
        string filePath = saveFolderPath + prefabName + ".png";
        File.WriteAllBytes(filePath, bytes);

        RenderTexture.active = null; // Reset the active RenderTexture
    }
}
