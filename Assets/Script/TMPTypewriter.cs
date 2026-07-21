using UnityEngine;
using TMPro;
using System.Collections;

public class TMPTypewriter : MonoBehaviour
{
    [Header("Typing Settings")]
    [Tooltip("Seconds between each character")]
    public float characterDelay = 0.04f;

    public bool playOnEnable = true;

    [Header("After Finished")]
    public bool disableAfterFinished = false;
    public float disableDelay = 3f;

    TMP_Text tmpText;
    string fullText;

    Coroutine typingRoutine;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        fullText = tmpText.text;
    }

    void OnEnable()
    {
        if (playOnEnable)
            StartTyping();
    }

    public void StartTyping()
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        tmpText.text = "";
        typingRoutine = StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {
        foreach (char c in fullText)
        {
            tmpText.text += c;
            yield return new WaitForSeconds(characterDelay);
        }

        typingRoutine = null;

        if (disableAfterFinished)
        {
            yield return new WaitForSeconds(disableDelay);
            gameObject.SetActive(false);
        }
    }
}
