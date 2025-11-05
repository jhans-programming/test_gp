using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger2 : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        GameManager.Instance.LoadNextScene(sceneName);
    }

    public void RestartScene()
    {
        GameManager.Instance.RestartScene();
    }
}
