using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
