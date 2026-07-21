using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey { get; private set; }

    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("Key collected!");
        Debug.Log("[Inventory] Key collected → hasKey = TRUE");
    }

    public void ResetInventory()
    {
        hasKey = false;
        Debug.Log("[Inventory] Reset → hasKey = FALSE");
    }
}
