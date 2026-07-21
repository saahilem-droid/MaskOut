using UnityEngine;
using System.Collections.Generic;

public class TutorialMaskHighlight : MonoBehaviour
{
    [Header("Mask These Platforms")]
    [Tooltip("Assign the exact platform tiles the player must mask")]
    public List<GameObject> platformsToMask = new List<GameObject>();

    [Header("Highlight")]
    public Color highlightColor = Color.yellow;
    public float pulseSpeed = 4f;

    Dictionary<GameObject, SpriteRenderer> renderers =
        new Dictionary<GameObject, SpriteRenderer>();

    Dictionary<GameObject, Color> originalColors =
        new Dictionary<GameObject, Color>();

    bool active;

    // -------------------------------------------------

    public void ActivateHighlight()
    {
        active = true;

        foreach (GameObject g in platformsToMask)
        {
            if (!g) continue;

            SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
            if (!sr) continue;

            renderers[g] = sr;
            originalColors[g] = sr.color;
        }
    }

    public void DeactivateHighlight()
    {
        active = false;

        foreach (var kv in renderers)
        {
            if (kv.Key && kv.Value)
                kv.Value.color = originalColors[kv.Key];
        }

        renderers.Clear();
        originalColors.Clear();
    }

    // -------------------------------------------------

    void Update()
    {
        if (!active)
            return;

        float pulse =
            Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed));

        foreach (var kv in renderers)
        {
            if (!kv.Key || !kv.Value)
                continue;

            kv.Value.color =
                Color.Lerp(
                    originalColors[kv.Key],
                    highlightColor,
                    pulse);
        }
    }

    // -------------------------------------------------

    public bool AllMasked()
    {
        foreach (GameObject g in platformsToMask)
        {
            if (g && g.activeSelf)
                return false;
        }

        return true;
    }
}
