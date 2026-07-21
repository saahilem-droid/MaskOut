using UnityEngine;

public class QuitConfirmPanelFX : MonoBehaviour
{
    public float fadeSpeed = 12f;
    public float scaleSpeed = 14f;

    CanvasGroup canvas;
    RectTransform rect;
    bool show;

    void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float targetAlpha = show ? 1f : 0f;
        Vector3 targetScale = show ? Vector3.one : Vector3.one * 0.9f;

        canvas.alpha = Mathf.Lerp(canvas.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        rect.localScale = Vector3.Lerp(rect.localScale, targetScale, Time.deltaTime * scaleSpeed);

        canvas.interactable = show;
        canvas.blocksRaycasts = show;

        // Optional auto-disable after hide
        if (!show && canvas.alpha < 0.01f)
            gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        show = true;
    }

    public void Hide()
    {
        show = false;
    }
}
