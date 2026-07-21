using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public LevelSelectPanelFX levelSelectFX;
    public QuitConfirmPanelFX quitConfirmFX;
    public CreditsPanelFX creditsFX;

    bool quitOpen;

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscape();
        }
    }

    void HandleEscape()
    {
        if (quitOpen)
        {
            quitConfirmFX.Hide();
            quitOpen = false;
            return;
        }

        quitConfirmFX.Show();
        quitOpen = true;
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);

        levelSelectFX.Hide();
        creditsFX.Hide();
        quitConfirmFX.Hide();

        quitOpen = false;
    }

    public void ShowLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectFX.Show();
    }

    public void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsFX.Show();
    }

    public void HideCredits()
    {
        creditsFX.Hide();
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Quit Game (Editor)");
#endif
    }

    public void CancelQuit()
    {
        quitConfirmFX.Hide();
        quitOpen = false;
    }
}
