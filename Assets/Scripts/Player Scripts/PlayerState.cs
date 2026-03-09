using UnityEngine;

/// <summary>
/// PlayerState — Shared Data Bus
///
/// The single source of truth for the entire player system.
/// Every component reads from and writes to this object.
/// No input lives here — each script owns its own keys.
///
/// HOW TO ADD A NEW ABILITY:
///   1. Add a state flag below (e.g. public bool IsWallJumping).
///   2. Create PlayerWallJump.cs — it reads/writes that flag.
///   3. Add one line to PlayerAnimator. Nothing else changes.
/// </summary>
public class PlayerState : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  References (set once by PlayerPhysics)
    // ─────────────────────────────────────────────
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D col;

    // ─────────────────────────────────────────────
    //  Collision (written by PlayerPhysics)
    // ─────────────────────────────────────────────
    [Header("Collision State")]
    public bool IsGrounded;
    public bool IsTouchingCeiling;
    public bool IsTouchingWallLeft;
    public bool IsTouchingWallRight;

    // ─────────────────────────────────────────────
    //  Movement (written by PlayerMovement)
    // ─────────────────────────────────────────────
    [Header("Movement State")]
    public float MoveInput;           // -1, 0, or 1  (written by PlayerMovement)
    public bool IsFacingRight = true;
    public float LastGroundedTime;    // Time.time stamp, useful for ability checks

    // ─────────────────────────────────────────────
    //  Jump (written by PlayerJump)
    // ─────────────────────────────────────────────
    [Header("Jump State")]
    public bool JumpHeld;             // true while jump key held (used by PlayerPhysics gravity)

    // ─────────────────────────────────────────────
    //  Ability flags (written by ability scripts)
    //  Read by Animator, other abilities, and Physics
    // ─────────────────────────────────────────────
    [Header("Ability State")]
    public bool IsDashing;
    public bool IsRolling;
    // Add more as needed:
    // public bool IsWallSliding;
    // public bool IsGliding;
    // public bool IsParrying;

    // ─────────────────────────────────────────────
    //  Control flags
    // ─────────────────────────────────────────────
    [Header("Control Flags")]
    /// <summary>
    /// When true, PlayerMovement and PlayerJump skip their logic.
    /// Set this to true at the START of any ability (dash, roll, cutscene).
    /// Set it back to false when the ability ends.
    /// Physics and collision detection always keep running.
    /// </summary>
    public bool OverrideMovement;

    /// <summary>
    /// Queue an impulse here from any ability script.
    /// PlayerPhysics applies it once per FixedUpdate then clears it.
    /// </summary>
    public Vector2 PendingImpulse;

    // ─────────────────────────────────────────────
    //  Convenience helpers
    // ─────────────────────────────────────────────

    public Vector2 Velocity
    {
        get => rb != null ? rb.linearVelocity : Vector2.zero;
        set { if (rb != null) rb.linearVelocity = value; }
    }

    /// <summary>Queue a one-shot velocity burst from any script.</summary>
    public void AddImpulse(Vector2 impulse) => PendingImpulse += impulse;

    public void SetHorizontalVelocity(float x) =>
        Velocity = new Vector2(x, Velocity.y);

    public bool IsDoingSomething { get; set; }

    public void SetVerticalVelocity(float y) =>
        Velocity = new Vector2(Velocity.x, y);
}