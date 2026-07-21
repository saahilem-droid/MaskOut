using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CubeController2D : MonoBehaviour
{
    // ================= AUDIO =================
[Header("Ground Check")]
[SerializeField] private string platformTag = "Platform";

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip rollLoopSound;

    [Range(0f, 1f)] public float jumpVolume = 0.8f;
    [Range(0f, 1f)] public float rollVolume = 0.6f;

    private AudioSource jumpSource;
    private AudioSource rollSource;
    private bool isRollSoundPlaying;

    // ================= MOVEMENT =================

    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float airRotationSpeed = 360f;
    public float cornerBounceForce = 2f;

    [Header("Jump Forgiveness")]
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Jump Lock")]
    public float jumpCooldown = 0.2f;

    // ================= VISUAL FX =================

    [Header("Breathing Effect")]
    public Transform visual;
    public float breathSpeed = 2f;
    public float breathAmount = 0.04f;
    public float breathDamp = 6f;

    [Header("Landing Squish")]
    public float landSquishAmount = 0.25f;
    public float landRecoverSpeed = 10f;
    public float landSquishSnap = 16f;

    [Header("Rotation Jelly")]
    public float jellyStretch = 0.12f;
    public float jellySpeed = 12f;
    public float jellyRecoverSpeed = 10f;
    public float maxAngularSpeed = 720f;

    // ================= PARTICLES =================

    [Header("Jump Particle")]
    public Transform feetPoint;
    public GameObject jumpParticlePrefab;

    // ================= INTERNAL =================

    Rigidbody2D rb;

    float moveInput;
    bool isGrounded;

    float coyoteCounter;
    float jumpBufferCounter;
    float jumpCooldownTimer;

    Vector3 visualBaseScale;
    float breathTimer;
    float breathBlend;

    float squishWeight;
    Vector3 jellyScaleOffset;

    // ================= SETUP =================

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length >= 2)
        {
            jumpSource = sources[0];
            rollSource = sources[1];
        }
        else
        {
            jumpSource = gameObject.AddComponent<AudioSource>();
            rollSource = gameObject.AddComponent<AudioSource>();
        }

        visualBaseScale = visual.localScale;
    }

    // ================= UPDATE =================

    void Update()
    {
        ReadInput();
        HandleRotation();
        HandleRotationJelly();
        HandleVisualEffects();
        HandleRollAudio();
    }

    void FixedUpdate()
    {
        jumpCooldownTimer -= Time.fixedDeltaTime;
        HandleMovement();
        HandleJump();
    }

    // ================= INPUT =================

    void ReadInput()
    {
        if (Keyboard.current == null) return;

        moveInput = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput = 1f;

        if (Keyboard.current.wKey.wasPressedThisFrame)
            jumpBufferCounter = jumpBufferTime;
    }

    // ================= MOVEMENT =================

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // ================= JUMP =================

    void HandleJump()
    {
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.fixedDeltaTime;

        jumpBufferCounter -= Time.fixedDeltaTime;

        if (jumpCooldownTimer > 0f)
            return;

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // ---- Particle ----
            if (jumpParticlePrefab != null && feetPoint != null)
            {
                GameObject fx = Instantiate(
                    jumpParticlePrefab,
                    feetPoint.position,
                    Quaternion.identity);

                Destroy(fx, 2f);
            }

            // ---- Jump Sound ----
            if (jumpSource != null && jumpSound != null)
                jumpSource.PlayOneShot(jumpSound, jumpVolume);

            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
            isGrounded = false;
            jumpCooldownTimer = jumpCooldown;
        }
    }

    // ================= ROLL AUDIO =================

    void HandleRollAudio()
    {
        if (rollSource == null || rollLoopSound == null)
            return;

        bool shouldPlay =
            isGrounded &&
            Mathf.Abs(moveInput) > 0.1f;

        if (shouldPlay)
        {
            if (!isRollSoundPlaying)
            {
                rollSource.clip = rollLoopSound;
                rollSource.volume = rollVolume;
                rollSource.loop = true;
                rollSource.Play();

                isRollSoundPlaying = true;
            }
        }
        else
        {
            if (isRollSoundPlaying)
            {
                rollSource.Stop();
                rollSource.loop = false;
                isRollSoundPlaying = false;
            }
        }
    }

    // ================= ROTATION =================

    void HandleRotation()
{
    float xSpeed = rb.linearVelocity.x;

    // --- On Ground ---
    if (isGrounded)
    {
        if (Mathf.Abs(xSpeed) > 0.05f)
        {
            float radius = transform.localScale.x * 0.5f;
            rb.angularVelocity = -(xSpeed / radius) * Mathf.Rad2Deg;
        }
        else
        {
            rb.angularVelocity = 0f;
        }
    }
    // --- In Air ---
    else
    {
        if (Mathf.Abs(xSpeed) > 0.05f)
        {
            float dir = Mathf.Sign(xSpeed);
            rb.angularVelocity = airRotationSpeed * -dir;
        }
        else
        {
            rb.angularVelocity = 0f;
        }
    }
}


    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag(platformTag))
        return;
        bool groundedNow = false;

        foreach (var c in collision.contacts)
        {
            if (c.normal.y > 0.5f)
            {
                groundedNow = true;
                break;
            }
        }

        if (!isGrounded && groundedNow)
        {
            squishWeight = 1f;
            
        }

        isGrounded = groundedNow;
    }

    // ================= JELLY =================

    void HandleRotationJelly()
    {
        if (!isGrounded)
        {
            jellyScaleOffset = Vector3.Lerp(
                jellyScaleOffset,
                Vector3.zero,
                Time.deltaTime * jellyRecoverSpeed);
            return;
        }

        float zRad = transform.eulerAngles.z * Mathf.Deg2Rad;

        float rotationFactor = Mathf.Abs(Mathf.Cos(zRad));

        float angularFactor =
            Mathf.Clamp01(Mathf.Abs(rb.angularVelocity) / maxAngularSpeed);

        float squash = rotationFactor * angularFactor * jellyStretch;

        jellyScaleOffset = Vector3.Lerp(
            jellyScaleOffset,
            new Vector3(squash, -squash, 0f),
            Time.deltaTime * jellySpeed);
    }

    // ================= VISUAL =================

    void HandleVisualEffects()
    {
        bool shouldBreathe =
            isGrounded &&
            Mathf.Abs(moveInput) < 0.01f &&
            Mathf.Abs(rb.linearVelocity.y) < 0.01f;

        float targetBlend = shouldBreathe ? 1f : 0f;

        breathBlend =
            Mathf.Lerp(breathBlend, targetBlend, Time.deltaTime * breathDamp);

        breathTimer += Time.deltaTime * breathSpeed;

        float breath = Mathf.Sin(breathTimer) * breathAmount * breathBlend;

        Vector3 breathOffset = new Vector3(breath, -breath, 0f);

        squishWeight =
            Mathf.MoveTowards(squishWeight, 0f,
                Time.deltaTime * landRecoverSpeed);

        Vector3 squishOffset =
            new Vector3(
                squishWeight * landSquishAmount,
                -squishWeight * landSquishAmount,
                0f);

        Vector3 targetScale =
            visualBaseScale +
            breathOffset +
            squishOffset +
            jellyScaleOffset;

        visual.localScale =
            Vector3.Lerp(
                visual.localScale,
                targetScale,
                Time.deltaTime * landSquishSnap);
    }
    void OnCollisionExit2D(Collision2D collision)
{
    if (collision.collider.CompareTag(platformTag))
    {
        isGrounded = false;
    }
}

}
