using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CinemachinePlayerLink : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (vCam == null)
            return;

        // Find the persistent player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            vCam.Follow = player.transform;
            vCam.LookAt = player.transform; // optional if you want the camera to look at the player
        }
    }
}
