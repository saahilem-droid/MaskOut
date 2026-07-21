using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Exact scene name as in Build Settings")]
    public string sceneName;

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name not set on " + gameObject.name);
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
