using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeathHandler : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string trapTag = "Traps";
    [SerializeField] private string borderTag = "Border";

    [Header("Death Effects")]
    public GameObject deathParticlePrefab;

    [Header("Death Audio")]
    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathVolume = 1f;

    [Header("Reload")]
    public float reloadDelay = 1.5f;

    bool dead;
    AudioSource audioSource;

    // -----------------------------

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    // -----------------------------
    // COLLISION DETECTION
    // -----------------------------

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead) return;

        if (collision.collider.CompareTag(trapTag) ||
            collision.collider.CompareTag(borderTag))
        {
            Debug.Log($"[Death] Collided with {collision.collider.tag}");
            KillPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dead) return;

        if (other.CompareTag(trapTag) ||
            other.CompareTag(borderTag))
        {
            Debug.Log($"[Death] Triggered by {other.tag}");
            KillPlayer();
        }
    }

    // -----------------------------

    public void KillPlayer()
    {
        if (dead)
            return;

        dead = true;

        Debug.Log("[Death] KillPlayer called");

        // ---- Spawn particles ----
        if (deathParticlePrefab != null)
        {
            Instantiate(
                deathParticlePrefab,
                transform.position,
                Quaternion.identity);
        }

        // ---- Play sound ----
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound, deathVolume);

        // ---- Disable player ----
        DisablePlayer();

        // ---- Reload scene ----
        StartCoroutine(ReloadRoutine());
    }

    // -----------------------------

    IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(reloadDelay);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    // -----------------------------

    void DisablePlayer()
    {
        var controller = GetComponent<CubeController2D>();
        if (controller) controller.enabled = false;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr) sr.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }
    }
}
