using UnityEngine;

public class LimitedMaskingPointTrigger : MonoBehaviour
{
    [SerializeField] private LimitedPlatformToggleManager manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            manager.PlayerEnteredMaskZone();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            manager.PlayerExitedMaskZone();
    }
}
