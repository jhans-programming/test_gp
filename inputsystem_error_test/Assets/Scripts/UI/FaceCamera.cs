using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera targetCamera;

    private void LateUpdate()
    {
        // Dynamically get the current active camera each frame
        if (targetCamera == null || !targetCamera.gameObject.activeInHierarchy)
        {
            targetCamera = Camera.main;
            if (targetCamera == null) return; // no camera found
        }

        // Rotate the health bar to face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.transform.position);
    }
}
