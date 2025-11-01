using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [Header("Camera Reference")]
    [SerializeField] private Camera targetCamera; // Assign in Inspector (optional)

    private void Start()
    {
        // Fallback to main camera if none is assigned
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        // Make the UI face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.transform.position);
    }
}
