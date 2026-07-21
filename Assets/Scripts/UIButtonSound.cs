using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public void PlayClick()
    {
        if (UISoundManager.Instance != null)
        {
            UISoundManager.Instance.PlayClick();
        }
    }
}
