using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeLoadScene : MonoBehaviour
{
    [Header("Scene to Load on Escape")]
    [SerializeField] private string sceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty! Assign it in the Inspector.");
        }
    }
}
