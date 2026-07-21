using UnityEngine;

public class HaloFollow : MonoBehaviour
{
    public Transform target;        // Cube transform
    public Vector3 offset = new Vector3(0f, 0.6f, 0f);

    [Header("Float Motion")]
    public float floatHeight = 0.08f;
    public float floatSpeed = 2f;

    float timer;

    void LateUpdate()
    {
        if (target == null) return;

        // Floating motion
        timer += Time.deltaTime * floatSpeed;
        float floatY = Mathf.Sin(timer) * floatHeight;

        // Follow position ONLY
        transform.position = target.position + offset + Vector3.up * floatY;

        // 🔒 Cancel rotation (critical)
        transform.rotation = Quaternion.identity;
    }
}
