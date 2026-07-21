using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonAction : MonoBehaviour
{
    public enum ActionType
    {
        Retry,
        LoadSceneByName
    }

    [Header("Button Action")]
    public ActionType action;

    [Tooltip("Used only when Action = LoadSceneByName")]
    public string sceneName;

    // -----------------------------

    public void Execute()
    {
        Time.timeScale = 1f;

        switch (action)
        {
            case ActionType.Retry:
            Debug.Log("retry initiated");
                ReloadCurrentScene();
                break;

            case ActionType.LoadSceneByName:
            Debug.Log("nxt scene initiated");
                LoadScene(sceneName);
                break;
        }
    }

    // -----------------------------

    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    void LoadScene(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("[SceneButtonAction] Scene name not set!");
            return;
        }

        SceneManager.LoadScene(name);
    }
}
