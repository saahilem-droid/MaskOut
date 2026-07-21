using UnityEngine;
using TMPro;
using System.Collections;

public class AutoDisableTMP : MonoBehaviour
{
    [Tooltip("Seconds after scene load before disabling this text")]
    public float disableDelay = 20f;

    TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        StartCoroutine(DisableRoutine());
    }

    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(disableDelay);

        if (text != null)
            text.gameObject.SetActive(false);
    }
}
