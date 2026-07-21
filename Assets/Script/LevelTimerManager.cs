using UnityEngine;
using TMPro;



public class LevelTimerManager : MonoBehaviour
{
    [Header("Timer UI")]
public TMP_Text timerText;

    [Header("Star Time Thresholds (seconds)")]
    [Tooltip("If elapsed time <= ThreeStarTime → 3 stars")]
    public float threeStarTime = 20f;

    [Tooltip("If elapsed time <= TwoStarTime → 2 stars")]
    public float twoStarTime = 35f;

    [Tooltip("Anything slower = 1 star")]
    public float oneStarTime = 999f;

    [Header("Win Panels")]
    public GameObject oneStarPanel;
    public GameObject twoStarPanel;
    public GameObject threeStarPanel;

    [Header("Options")]
    public bool pauseOnWin = true;

    float elapsedTime;
    bool running = true;

    // -----------------------------

    void Start()
    {
        elapsedTime = 0f;
        running = true;

        DisableAllPanels();
    }

    void Update()
{
    if (!running)
        return;

    elapsedTime += Time.deltaTime;

    UpdateTimerUI();
}

void UpdateTimerUI()
{
    if (timerText == null)
        return;

    timerText.text = FormatTime(elapsedTime);
}

string FormatTime(float t)
{
    int minutes = Mathf.FloorToInt(t / 60f);
    int seconds = Mathf.FloorToInt(t % 60f);
    int millis = Mathf.FloorToInt((t * 1000f) % 1000f);

    return $"{minutes:00}:{seconds:00}.{millis:000}";
}


    // -----------------------------

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // -----------------------------

    public void LevelCompleted()
    {
        running = false;
        UpdateTimerUI();

        DisableAllPanels();

        Debug.Log($"[Timer] Level completed in {elapsedTime:F2} seconds");

        // Choose star tier
        if (elapsedTime <= threeStarTime && threeStarPanel != null)
        {
            Debug.Log("[Timer] 3 Star Panel");
            threeStarPanel.SetActive(true);
        }
        else if (elapsedTime <= twoStarTime && twoStarPanel != null)
        {
            Debug.Log("[Timer] 2 Star Panel");
            twoStarPanel.SetActive(true);
        }
        else
        {
            Debug.Log("[Timer] 1 Star Panel");
            if (oneStarPanel != null)
                oneStarPanel.SetActive(true);
        }

        if (pauseOnWin)
            Time.timeScale = 0f;
    }

    // -----------------------------

    void DisableAllPanels()
    {
        if (oneStarPanel) oneStarPanel.SetActive(false);
        if (twoStarPanel) twoStarPanel.SetActive(false);
        if (threeStarPanel) threeStarPanel.SetActive(false);
    }
}
