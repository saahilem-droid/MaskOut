using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerInventory inv =
            other.GetComponentInParent<PlayerInventory>();

        if (inv == null)
            return;

        if (inv.hasKey)
        {
            Debug.Log("[Door] Player has key — completing level");

            LevelTimerManager timer =
                FindObjectOfType<LevelTimerManager>();

            if (timer != null)
                timer.LevelCompleted();
            else
                Debug.LogError("[Door] No LevelTimerManager in scene!");
        }
        else
        {
            Debug.Log("[Door] Door locked — no key");
        }
    }
}
