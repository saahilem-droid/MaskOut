using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public float speed = 2f;
    public float scaleAmount = 0.15f;

    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float t = 1f + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = baseScale * t;
    }
}
