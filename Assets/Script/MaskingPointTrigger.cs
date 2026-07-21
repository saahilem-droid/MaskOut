using UnityEngine;

public class MaskingPointTrigger : MonoBehaviour
{
    [SerializeField] private MaskPointPlatformToggleManager manager;

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
