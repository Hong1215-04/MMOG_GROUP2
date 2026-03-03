using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement2D : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Inspector – Key Bindings
    // ─────────────────────────────────────────────
    [Header("Key Bindings")]
    [Tooltip("Move left")]
    public KeyCode keyLeft = KeyCode.A;

    [Tooltip("Move right")]
    public KeyCode keyRight = KeyCode.D;

    [Tooltip("Jump")]
    public KeyCode keyJump = KeyCode.Space;

    // ─────────────────────────────────────────────
    //  Inspector – Movement
    // ─────────────────────────────────────────────
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float groundAcceleration = 80f;
    public float groundDeceleration = 100f;
    public float airAcceleration = 50f;
    public float airDeceleration = 30f;

    // ─────────────────────────────────────────────
    //  Inspector – Jump
    // ─────────────────────────────────────────────
    [Header("Jump")]
    public float jumpForce = 16f;
    public float jumpHoldGravityScale = 1.5f;
    public float fallGravityScale = 3.5f;
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.15f;
    public float maxFallSpeed = 25f;

    // ─────────────────────────────────────────────
    //  Inspector – Collision Detection
    // ─────────────────────────────────────────────
    [Header("Collision Detection")]
    public LayerMask collisionMask;
    public float groundCheckDistance = 0.08f;
    public float ceilingCheckDistance = 0.05f;
    public float wallCheckDistance = 0.06f;
    [Range(2, 8)]
    public int rayCount = 4;
    public float skinWidth = 0.02f;
    public float maxSlopeAngle = 46f;

    // ─────────────────────────────────────────────
    //  Public State
    // ─────────────────────────────────────────────
    public bool IsGrounded { get; private set; }
    public bool IsTouchingCeiling { get; private set; }
    public bool IsTouchingWallLeft { get; private set; }
    public bool IsTouchingWallRight { get; private set; }
    public bool IsFacingRight { get; private set; } = true;

    public Vector2 Velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    /// <summary>
    /// Disable internal movement from an external script (dash, roll, etc.).
    /// Collision state keeps updating while this is true.
    /// </summary>
    public bool OverrideMovement { get; set; } = false;

    /// <summary>
    /// Queue a one-shot impulse from an external script.
    /// Applied once at the top of the next FixedUpdate then cleared.
    /// </summary>
    public Vector2 ExternalImpulse { get; set; } = Vector2.zero;

    // ─────────────────────────────────────────────
    //  Private
    // ─────────────────────────────────────────────
    Rigidbody2D rb;
    Collider2D col;

    float coyoteTimer;
    float jumpBufferTimer;
    bool jumpHeld;
    bool wasGroundedLastFrame;
    float _moveInput;

    Bounds Bounds => col.bounds;

    // ─────────────────────────────────────────────
    //  Unity lifecycle
    // ─────────────────────────────────────────────
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // ── Timers ───────────────────────────────
        coyoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;

        // ── Read keys ────────────────────────────
        float moveInput = 0f;
        if (Input.GetKey(keyRight)) moveInput += 1f;
        if (Input.GetKey(keyLeft)) moveInput -= 1f;

        bool jumpPressed = Input.GetKeyDown(keyJump);
        jumpHeld = Input.GetKey(keyJump);

        if (jumpPressed)
            jumpBufferTimer = jumpBufferTime;

        // ── Flip to face movement direction ──────
        if (!OverrideMovement)
        {
            if (moveInput > 0f && !IsFacingRight)
            {
                IsFacingRight = true;
                Flip();
            }
            else if (moveInput < 0f && IsFacingRight)
            {
                IsFacingRight = false;
                Flip();
            }
        }

        // ── Jump ─────────────────────────────────
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            DoJump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        _moveInput = moveInput;
    }

    void FixedUpdate()
    {
        DetectCollisions();

        // Coyote time
        if (IsGrounded)
        {
            coyoteTimer = coyoteTime;
            wasGroundedLastFrame = true;
        }
        else if (wasGroundedLastFrame)
        {
            wasGroundedLastFrame = false;
        }

        // Ceiling bump
        if (IsTouchingCeiling && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // External impulse
        if (ExternalImpulse != Vector2.zero)
        {
            rb.linearVelocity += ExternalImpulse;
            ExternalImpulse = Vector2.zero;
        }

        if (OverrideMovement) return;

        ApplyHorizontalMovement(_moveInput);
        ApplyGravity();
    }

    // ─────────────────────────────────────────────
    //  Movement
    // ─────────────────────────────────────────────

    void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void ApplyHorizontalMovement(float input)
    {
        float accel = IsGrounded ? groundAcceleration : airAcceleration;
        float decel = IsGrounded ? groundDeceleration : airDeceleration;
        float targetSpeed = input * maxSpeed;
        float currentX = rb.linearVelocity.x;
        float newX;

        if (Mathf.Abs(input) > 0.01f)
        {
            newX = Mathf.MoveTowards(currentX, targetSpeed, accel * Time.fixedDeltaTime);

            if ((newX > 0f && IsTouchingWallRight) ||
                (newX < 0f && IsTouchingWallLeft))
                newX = 0f;
        }
        else
        {
            newX = Mathf.MoveTowards(currentX, 0f, decel * Time.fixedDeltaTime);
        }

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    void ApplyGravity()
    {
        if (IsGrounded && rb.linearVelocity.y <= 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            return;
        }

        float gravScale = (rb.linearVelocity.y > 0f && jumpHeld)
            ? jumpHoldGravityScale
            : fallGravityScale;

        Vector2 gravity = Physics2D.gravity * gravScale * Time.fixedDeltaTime;
        Vector2 newVel = rb.linearVelocity + gravity;
        newVel.y = Mathf.Max(newVel.y, -maxFallSpeed);

        rb.linearVelocity = newVel;
    }

    // ─────────────────────────────────────────────
    //  Flip / facing
    // ─────────────────────────────────────────────

    void Flip()
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (IsFacingRight ? 1f : -1f);
        transform.localScale = s;
    }

    /// <summary>Force a facing direction from an external script.</summary>
    public void FlipFacing(bool faceRight)
    {
        if (IsFacingRight == faceRight) return;
        IsFacingRight = faceRight;
        Flip();
    }

    // ─────────────────────────────────────────────
    //  Raycast collision detection
    // ─────────────────────────────────────────────

    void DetectCollisions()
    {
        Bounds b = Bounds;
        IsGrounded = CheckGround(b);
        IsTouchingCeiling = CheckCeiling(b);
        IsTouchingWallLeft = CheckWall(b, -1);
        IsTouchingWallRight = CheckWall(b, 1);
    }

    bool CheckGround(Bounds b)
    {
        float left = b.min.x + skinWidth;
        float right = b.max.x - skinWidth;
        float y = b.min.y;

        for (int i = 0; i < rayCount; i++)
        {
            float t = rayCount > 1 ? (float)i / (rayCount - 1) : 0.5f;
            var origin = new Vector2(Mathf.Lerp(left, right, t), y);
            var hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance + skinWidth, collisionMask);

            Debug.DrawRay(origin, Vector2.down * (groundCheckDistance + skinWidth), Color.green);

            if (hit.collider != null && Vector2.Angle(hit.normal, Vector2.up) <= maxSlopeAngle)
                return true;
        }
        return false;
    }

    bool CheckCeiling(Bounds b)
    {
        float left = b.min.x + skinWidth;
        float right = b.max.x - skinWidth;
        float y = b.max.y;

        for (int i = 0; i < rayCount; i++)
        {
            float t = rayCount > 1 ? (float)i / (rayCount - 1) : 0.5f;
            var origin = new Vector2(Mathf.Lerp(left, right, t), y);
            var hit = Physics2D.Raycast(origin, Vector2.up, ceilingCheckDistance + skinWidth, collisionMask);

            Debug.DrawRay(origin, Vector2.up * (ceilingCheckDistance + skinWidth), Color.red);

            if (hit.collider != null) return true;
        }
        return false;
    }

    bool CheckWall(Bounds b, int dir)
    {
        float x = dir < 0 ? b.min.x : b.max.x;
        float bot = b.min.y + skinWidth;
        float top = b.max.y - skinWidth;
        var xDir = dir < 0 ? Vector2.left : Vector2.right;

        for (int i = 0; i < rayCount; i++)
        {
            float t = rayCount > 1 ? (float)i / (rayCount - 1) : 0.5f;
            var origin = new Vector2(x, Mathf.Lerp(bot, top, t));
            var hit = Physics2D.Raycast(origin, xDir, wallCheckDistance + skinWidth, collisionMask);

            Debug.DrawRay(origin, xDir * (wallCheckDistance + skinWidth), Color.blue);

            if (hit.collider != null) return true;
        }
        return false;
    }

    // ─────────────────────────────────────────────
    //  Public helpers for external scripts
    // ─────────────────────────────────────────────

    /// <summary>Queue a velocity burst (dash, knockback, etc.).</summary>
    public void AddImpulse(Vector2 impulse) => ExternalImpulse += impulse;

    /// <summary>Override only the vertical velocity component.</summary>
    public void SetVerticalVelocity(float y) =>
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);

    /// <summary>Override only the horizontal velocity component.</summary>
    public void SetHorizontalVelocity(float x) =>
        rb.linearVelocity = new Vector2(x, rb.linearVelocity.y);
}