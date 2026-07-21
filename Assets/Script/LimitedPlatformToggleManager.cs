using System.Collections.Generic;
using UnityEngine;

public class LimitedPlatformToggleManager : MonoBehaviour
{
    [Header("Masking Gate")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string maskingPointTag = "MaskingPoint";

    [Header("Global Mask Limit")]
    [SerializeField] private int maxMasksAllowed = 5;

    private bool insideMaskZone = false;
    private bool maskingLockedForever = false;

    private int masksUsed = 0;

    [Header("Keys")]
    public KeyCode selectHoldKey = KeyCode.F;

    [Header("Maskable Tags")]
    public string platformTag = "Platform";
    [SerializeField] private string trapsTag = "Traps";

    private bool wasHoldingSelectKey = false;

    private HashSet<GameObject> selectedPlatforms = new HashSet<GameObject>();
    private List<GameObject> allPlatforms = new List<GameObject>();

    // ------------------------------------------------------

    public void PlayerEnteredMaskZone()
    {
        if (!maskingLockedForever)
            insideMaskZone = true;
    }

    public void PlayerExitedMaskZone()
    {
        insideMaskZone = false;

        // permanently disable masking for this run
        maskingLockedForever = true;
    }

    // ------------------------------------------------------

    void Start()
    {
        CacheAllPlatforms();
    }

    void CacheAllPlatforms()
{
    allPlatforms.Clear();

    GameObject[] platforms =
        GameObject.FindGameObjectsWithTag(platformTag);

    GameObject[] traps =
        GameObject.FindGameObjectsWithTag(trapsTag);

    foreach (GameObject g in platforms)
        allPlatforms.Add(g);

    foreach (GameObject g in traps)
        allPlatforms.Add(g);
}

    // ------------------------------------------------------

    void Update()
    {
        HandleSelectionInput();
        HandleToggleInput();
    }

    // ------------------------------------------------------

    void HandleSelectionInput()
    {
        // ---- HARD GATE ----
        if (!insideMaskZone || maskingLockedForever)
            return;

        // Global mask limit fully reached
        if (masksUsed >= maxMasksAllowed)
            return;

        bool holding = Input.GetKey(selectHoldKey);

        // Click to toggle selection (respect remaining quota)
        if (holding && Input.GetMouseButtonDown(0))
        {
            int remaining = maxMasksAllowed - masksUsed;

            if (selectedPlatforms.Count >= remaining)
                return;

            Vector2 mouseWorld =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            SelectSingleAtPoint(mouseWorld);
        }

        // Release F → APPLY MASK
        if (wasHoldingSelectKey && !holding)
        {
            int newlyMasked = DisableSelectedPlatforms();
            masksUsed += newlyMasked;

            ClearSelectionVisuals();
        }

        wasHoldingSelectKey = holding;
    }

    // ------------------------------------------------------

    void ClearSelectionVisuals()
    {
        foreach (GameObject g in selectedPlatforms)
        {
            SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
            if (sr)
                sr.color = Color.white;
        }

        selectedPlatforms.Clear();
    }

    // ------------------------------------------------------

    void SelectSingleAtPoint(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);

        if (!hit.collider)
            return;

        if (!hit.collider.CompareTag(platformTag) &&
            !hit.collider.CompareTag(trapsTag))
            return;

        TogglePlatformSelection(hit.collider.gameObject);
    }

    // ------------------------------------------------------

    void TogglePlatformSelection(GameObject platform)
    {
        SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();

        if (selectedPlatforms.Contains(platform))
        {
            selectedPlatforms.Remove(platform);

            if (sr)
                sr.color = Color.white;
        }
        else
        {
            selectedPlatforms.Add(platform);

            if (sr)
                sr.color = Color.red;
        }
    }

    // ------------------------------------------------------

    int DisableSelectedPlatforms()
    {
        int count = 0;

        foreach (GameObject g in selectedPlatforms)
        {
            if (!g.activeSelf)
                continue;

            g.SetActive(false);
            count++;
        }

        return count;
    }

    // ------------------------------------------------------

    void HandleToggleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ToggleAllPlatforms();
        }
    }

    // ------------------------------------------------------

    void ToggleAllPlatforms()
    {
        foreach (GameObject g in allPlatforms)
        {
            g.SetActive(!g.activeSelf);
        }
    }

    // ------------------------------------------------------

    public void ResetSelectionSystem()
    {
        wasHoldingSelectKey = false;

        selectedPlatforms.Clear();

        foreach (GameObject g in allPlatforms)
        {
            if (!g.activeSelf)
                g.SetActive(true);

            SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
            if (sr)
                sr.color = Color.white;
        }

        masksUsed = 0;
        insideMaskZone = false;
        maskingLockedForever = false;
    }
}
