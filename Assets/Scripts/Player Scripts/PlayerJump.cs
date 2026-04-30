using System;
using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerJump : MonoBehaviour
{
    [Header("Key Bindings")]
    public KeyCode keyJump = KeyCode.Space;
    public Action onJumped;
    bool Jump = true;

    [Header("Jump")]
    public float jumpForce = 16f;
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.15f;

    [Header("Multi Jump")]
    public int maxJumps = 2; // 1 = no double jump, 2 = one extra air jump, etc.
    int jumpsRemaining;

    // ─────────────────────────────────────────────
    PlayerState state;
    float coyoteTimer;
    float jumpBufferTimer;

    void Awake() => state = GetComponent<PlayerState>();

    void Update()
    {
        if (state.IsStunned) return;
        if (!Jump) return;

        bool jumpPressed = Input.GetKeyDown(keyJump);
        state.JumpHeld = Input.GetKey(keyJump);

        coyoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;

        if (jumpPressed)
            jumpBufferTimer = jumpBufferTime;

        // Refresh coyote window and jumps when grounded
        if (state.IsGrounded)
        {
            coyoteTimer = coyoteTime;
            jumpsRemaining = maxJumps;
        }

        if (!state.OverrideMovement && jumpBufferTimer > 0f)
        {
            // First jump — requires coyote window (normal grounded jump)
            if (coyoteTimer > 0f)
            {
                DoJump();
                jumpBufferTimer = 0f;
                coyoteTimer = 0f;
            }
            // Air jumps — coyote expired but still have jumps left
            else if (jumpsRemaining > 0)
            {
                DoJump();
                jumpBufferTimer = 0f;
            }
        }
    }

    public void DoJump()
    {
        state.SetVerticalVelocity(jumpForce);
        jumpsRemaining = Mathf.Max(0, jumpsRemaining - 1);
        onJumped?.Invoke();
    }

    public void ForceJump(float? forceOverride = null)
    {
        state.SetVerticalVelocity(forceOverride ?? jumpForce);
    }

    public void CannotJump() => Jump = false;
    public void CanJump() => Jump = true;
}