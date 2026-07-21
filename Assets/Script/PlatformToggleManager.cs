using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlatformToggleManager : MonoBehaviour
{
   [Header("World Drag Visual")]
[SerializeField] Transform dragVisual;
[SerializeField] SpriteRenderer dragRenderer;

private Vector2 dragStartWorld;
private Vector2 dragCurrentWorld;



    [Header("Drag Selection")]
public float clickThreshold = 0.15f;

private bool isDragging = false;

private Vector2 dragEndWorld;

    [Header("Keys")]
    public KeyCode selectHoldKey = KeyCode.F;
    

    [Header("Settings")]
    public string platformTag = "Platform";
    [SerializeField] private string trapsTag = "Traps";

    
    private bool wasHoldingSelectKey = false;

    private HashSet<GameObject> selectedPlatforms = new HashSet<GameObject>();
    private List<GameObject> allPlatforms = new List<GameObject>();

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

    void Update()
    {
        HandleSelectionInput();
        HandleToggleInput();
    }

    // -------------------------------------------------------

Vector2 GetMouseWorldClamped()
{
    Camera cam = Camera.main;

    Vector3 mp = Input.mousePosition;

    mp.x = Mathf.Clamp(mp.x, 0, Screen.width);
    mp.y = Mathf.Clamp(mp.y, 0, Screen.height);

    Vector3 world = cam.ScreenToWorldPoint(mp);
    world.z = 0;

    return world;
}


    // -------------------------------------------------------
void UpdateDragVisual()
{
    Vector2 min = Vector2.Min(dragStartWorld, dragCurrentWorld);
    Vector2 max = Vector2.Max(dragStartWorld, dragCurrentWorld);

    Vector2 size = max - min;
    Vector2 center = (min + max) * 0.5f;

    dragVisual.position = center;
    dragVisual.localScale = new Vector3(size.x, size.y, 1f);
}

    void HandleSelectionInput()
{
    bool holding = Input.GetKey(selectHoldKey);

    // Start drag
    if (holding && Input.GetMouseButtonDown(0))
{
    isDragging = true;

    dragStartWorld = GetMouseWorldClamped();
    dragCurrentWorld = dragStartWorld;

    dragVisual.gameObject.SetActive(true);
    UpdateDragVisual();
}



    // Update drag UI
    if (holding && isDragging && Input.GetMouseButton(0))
{
    dragCurrentWorld = GetMouseWorldClamped();
    UpdateDragVisual();
}

    // End drag
    if (isDragging && Input.GetMouseButtonUp(0))
{
    dragCurrentWorld = GetMouseWorldClamped();

    dragVisual.gameObject.SetActive(false);

    float dist = Vector2.Distance(dragStartWorld, dragCurrentWorld);

    if (dist < clickThreshold)
        SelectSingleAtPoint(dragCurrentWorld);
    else
        SelectByDrag(dragStartWorld, dragCurrentWorld);

    isDragging = false;
}



    // Released F → APPLY MASK EVERY TIME
    if (wasHoldingSelectKey && !holding)
    {
        DisableSelectedPlatforms();
        ClearSelectionVisuals();
    }

    wasHoldingSelectKey = holding;
}

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

void SelectByDrag(Vector2 start, Vector2 end)
{
    Vector2 min = Vector2.Min(start, end);
    Vector2 max = Vector2.Max(start, end);

    Collider2D[] hits = Physics2D.OverlapAreaAll(min, max);

    foreach (Collider2D c in hits)
    {
        if (!c.CompareTag(platformTag))
            continue;

        TogglePlatformSelection(c.gameObject);
    }
}

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


    // -------------------------------------------------------

    void SelectPlatformUnderMouse()
{
    Vector2 mouseWorld =
        Camera.main.ScreenToWorldPoint(Input.mousePosition);

    RaycastHit2D hit =
        Physics2D.Raycast(mouseWorld, Vector2.zero);

    if (!hit.collider)
        return;

    if (!hit.collider.CompareTag(platformTag))
        return;

    GameObject platform = hit.collider.gameObject;

    SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();

    // ----------------------------------
    // TOGGLE SELECTION
    // ----------------------------------

    if (selectedPlatforms.Contains(platform))
    {
        // DESELECT
        selectedPlatforms.Remove(platform);

        if (sr)
            sr.color = Color.white;
    }
    else
    {
        // SELECT
        selectedPlatforms.Add(platform);

        if (sr)
            sr.color = Color.red;
    }
}


    // -------------------------------------------------------

    void DisableSelectedPlatforms()
    {
        foreach (GameObject g in selectedPlatforms)
        {
            g.SetActive(false);
        }
    }

    // -------------------------------------------------------

    void HandleToggleInput()
{
    if (Input.GetMouseButtonDown(1))
    {
        ToggleAllPlatforms();
    }
}



    // -------------------------------------------------------

    void ToggleAllPlatforms()
    {
        foreach (GameObject g in allPlatforms)
        {
            g.SetActive(!g.activeSelf);
        }
    }

    // -------------------------------------------------------
    // CALL THIS ON FAIL / TIMER EXPIRE
    // -------------------------------------------------------

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
    }
}
