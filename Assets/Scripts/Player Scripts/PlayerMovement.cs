using UnityEngine;

/// <summary>
/// PlayerMovement — Horizontal Movement + Sprite Facing
///
/// Owns its own left/right key bindings.
/// Sole responsibilities:
///   - Read horizontal input and write state.MoveInput
///   - Accelerate / decelerate horizontally
///   - Flip localScale.x to face the movement direction
///
/// Skips movement/facing when state.OverrideMovement is true.
/// </summary>
[RequireComponent(typeof(PlayerState))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyRight = KeyCode.D;

    bool Move = true;

    [Header("Movement")]
    public float maxSpeed = 8f;
    public float groundAcceleration = 80f;
    public float groundDeceleration = 100f;
    public float airAcceleration = 50f;
    public float airDeceleration = 30f;

    // ─────────────────────────────────────────────
    PlayerState state;

    // Each active effect adds its multiplier to this list.
    // Final speed = maxSpeed * all multipliers combined.
    // Automatically returns to normal when all effects expire.
    readonly System.Collections.Generic.List<float> speedMultipliers = new();

    public float SpeedMultiplier
    {
        get
        {
            float result = 1f;
            foreach (float m in speedMultipliers) result *= m;
            return result;
        }
    }

    public void AddSpeedMultiplier(float multiplier) => speedMultipliers.Add(multiplier);
    public void RemoveSpeedMultiplier(float multiplier) => speedMultipliers.Remove(multiplier);

    void Awake() => state = GetComponent<PlayerState>();

    void Update()
    {
        // Read horizontal input — always update MoveInput so other scripts can see it
        state.MoveInput = 0f;
        if (Input.GetKey(keyRight)) state.MoveInput += 1f;
        if (Input.GetKey(keyLeft)) state.MoveInput -= 1f;

        if (state.OverrideMovement) return;
        UpdateFacing();
    }

    void FixedUpdate()
    {
        if (state.OverrideMovement) return;
        if (!Move) return;
        //OverrideMovemennt might used for the others' movement function such as dash
        ApplyHorizontalMovement(); 
    }

    // ─────────────────────────────────────────────
    //  Facing
    // ─────────────────────────────────────────────

    void UpdateFacing()
    {
        if (state.MoveInput > 0f && !state.IsFacingRight)
            SetFacing(true);
        else if (state.MoveInput < 0f && state.IsFacingRight)
            SetFacing(false);
    }

    void SetFacing(bool faceRight)
    {
        state.IsFacingRight = faceRight;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (faceRight ? 1f : -1f);
        transform.localScale = s;
    }

    /// <summary>
    /// Force a facing change from an external script (e.g. dash starts left).
    /// </summary>
    public void FlipFacing(bool faceRight)
    {
        if (state.IsFacingRight != faceRight)
            SetFacing(faceRight);
    }

    // ─────────────────────────────────────────────
    //  Horizontal movement
    // ─────────────────────────────────────────────

    void ApplyHorizontalMovement()
    {
        float accel = state.IsGrounded ? groundAcceleration : airAcceleration;
        float decel = state.IsGrounded ? groundDeceleration : airDeceleration;
        float targetSpeed = state.MoveInput * maxSpeed * SpeedMultiplier;
        float currentX = state.Velocity.x;
        float newX;

        if (Mathf.Abs(state.MoveInput) > 0.01f)
        {
            newX = Mathf.MoveTowards(currentX, targetSpeed, accel * Time.fixedDeltaTime);

            // Don't push into a wall already being touched
            if ((newX > 0f && state.IsTouchingWallRight) ||
                (newX < 0f && state.IsTouchingWallLeft))
                newX = 0f;
        }
        else
        {
            newX = Mathf.MoveTowards(currentX, 0f, decel * Time.fixedDeltaTime);
        }

        state.SetHorizontalVelocity(newX);
    }

    public void CanMove()
    {
        Move = true;
    }
    public void CannotMove()
    {
        Move = false;
    }
}