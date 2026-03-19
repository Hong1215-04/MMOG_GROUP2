using System;
using UnityEngine;

/// <summary>
/// PlayerJump — Jump Logic
///
/// Owns its own jump key binding.
/// Sole responsibilities:
///   - Read jump input and write state.JumpHeld
///   - Track coyote time and jump buffer
///   - Execute a jump when conditions are met
///
/// Skips when state.OverrideMovement is true.
/// </summary>
[RequireComponent(typeof(PlayerState))]
public class PlayerJump : MonoBehaviour
{
    [Header("Key Bindings")]
    public KeyCode keyJump = KeyCode.Space;
    public Action onJumped;
    bool CanJump = true;

    [Header("Jump")]
    public float jumpForce = 16f;
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.15f;

    public bool jumped = false;

    // ─────────────────────────────────────────────
    PlayerState state;
    float coyoteTimer;
    float jumpBufferTimer;

    void Awake() => state = GetComponent<PlayerState>();

    void Update()
    {
        // Always read input so JumpHeld is accurate for gravity in PlayerPhysics
        bool jumpPressed = Input.GetKeyDown(keyJump);
        state.JumpHeld = Input.GetKey(keyJump);

        // Timers always count down
        coyoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;

        // Buffer the jump press
        if (jumpPressed)
            jumpBufferTimer = jumpBufferTime;

        // Refresh coyote window when grounded
        if (state.IsGrounded)
            coyoteTimer = coyoteTime;

        // Attempt jump
        if (!state.OverrideMovement && jumpBufferTimer > 0f && coyoteTimer > 0f && CanJump)
        {
            DoJump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }
    }

    public void DoJump()
    {
         state.SetVerticalVelocity(jumpForce);
         onJumped?.Invoke();
    }

    /// <summary>
    /// Grant an extra jump from an external script (double jump, bounce pad, etc.).
    /// </summary>
    public void ForceJump(float? forceOverride = null)
    {
        state.SetVerticalVelocity(forceOverride ?? jumpForce);
    }
}