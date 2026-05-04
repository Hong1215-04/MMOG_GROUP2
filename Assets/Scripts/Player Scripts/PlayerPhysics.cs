using UnityEngine;

/// <summary>
/// PlayerPhysics — Collision Detection + Gravity + Impulse Application
///
/// Sole responsibilities:
///   - Cache Rigidbody2D and Collider2D onto PlayerState
///   - Run raycast-based collision detection every FixedUpdate
///   - Apply custom gravity
///   - Apply any PendingImpulse queued by other scripts
///
/// This script runs REGARDLESS of OverrideMovement — collision state
/// must always be accurate so ability scripts know what's happening.
/// </summary>
[RequireComponent(typeof(PlayerState))]
public class PlayerPhysics : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Inspector – Collision
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

    [Header("Gravity")]
    public float jumpHoldGravityScale = 1.5f;
    public float fallGravityScale = 3.5f;
    public float maxFallSpeed = 25f;

    // ─────────────────────────────────────────────
    PlayerState state;
    Bounds Bounds => state.col.bounds;

    void Awake()
    {
        state = GetComponent<PlayerState>();
        state.rb = GetComponent<Rigidbody2D>();
        

        state.rb.gravityScale = 0f;
        state.rb.freezeRotation = true;
        state.rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        state.rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void FixedUpdate()
    {
        // Always run — abilities need accurate collision state
        DetectCollisions();
        ApplyPendingImpulse();

        // Ceiling bump
        if (state.IsTouchingCeiling && state.Velocity.y > 0f)
            state.SetVerticalVelocity(0f);

        // Gravity runs even during abilities (unless they set velocity themselves)
        if (!state.OverrideMovement)
            ApplyGravity();
    }

    // ─────────────────────────────────────────────
    //  Gravity
    // ─────────────────────────────────────────────

    void ApplyGravity()
    {
        if (state.IsGrounded && state.Velocity.y <= 0f)
        {
            state.SetVerticalVelocity(0f);
            return;
        }

        float gravScale = (state.Velocity.y > 0f && state.JumpHeld)
            ? jumpHoldGravityScale
            : fallGravityScale;

        Vector2 newVel = state.Velocity + Physics2D.gravity * gravScale * Time.fixedDeltaTime;
        newVel.y = Mathf.Max(newVel.y, -maxFallSpeed);
        state.Velocity = newVel;
    }

    // ─────────────────────────────────────────────
    //  Impulse
    // ─────────────────────────────────────────────

    void ApplyPendingImpulse()
    {
        if (state.PendingImpulse == Vector2.zero) return;
        state.Velocity += state.PendingImpulse;
        state.PendingImpulse = Vector2.zero;
    }

    // ─────────────────────────────────────────────
    //  Raycast collision detection
    // ─────────────────────────────────────────────

    void DetectCollisions()
    {
        Bounds b = Bounds;
        state.IsGrounded = CheckGround(b);
        state.IsTouchingCeiling = CheckCeiling(b);
        state.IsTouchingWallLeft = CheckWall(b, -1);
        state.IsTouchingWallRight = CheckWall(b, 1);

        if (state.IsGrounded)
            state.LastGroundedTime = Time.time;
    }

    bool CheckGround(Bounds b)
    {
        float left = b.min.x + skinWidth;
        float right = b.max.x - skinWidth;
        // Start slightly inside the collider so the ray punches through
        // the bottom edge — prevents bobbing caused by the physics skin gap
        float originY = b.min.y + skinWidth;

        for (int i = 0; i < rayCount; i++)
        {
            float t = rayCount > 1 ? (float)i / (rayCount - 1) : 0.5f;
            var origin = new Vector2(Mathf.Lerp(left, right, t), originY);
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

        for (int i = 0; i < rayCount; i++)
        {
            float t = rayCount > 1 ? (float)i / (rayCount - 1) : 0.5f;
            var origin = new Vector2(Mathf.Lerp(left, right, t), b.max.y);
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
}