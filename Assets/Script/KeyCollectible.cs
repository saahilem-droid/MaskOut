using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Key] HIT: {other.gameObject.name}");

        var invSame = other.GetComponent<PlayerInventory>();
        Debug.Log("[Key] Inventory on SAME object = " + invSame);

        var invParent = other.GetComponentInParent<PlayerInventory>();
        Debug.Log("[Key] Inventory in PARENT = " + invParent);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("[Key] Not player — ignored");
            return;
        }

        if (invParent == null)
        {
            Debug.LogError("[Key] NO PlayerInventory FOUND ANYWHERE!");
            return;
        }

        invParent.CollectKey();
        Debug.Log("[Key] Key collected successfully");

        Destroy(gameObject);
    }
}
