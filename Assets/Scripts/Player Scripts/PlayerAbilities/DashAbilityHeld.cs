using UnityEngine;

public class DashAbilityHeld : PlayerAbility
{
    [SerializeField] PlayerState state;
    [SerializeField] PlayerAnimator playerAnimator;
    [SerializeField] string dashAnimParam = "Dash";

    [Header("Dash Settings")]
    [SerializeField] float dashForce = 70f;
    // "uses" in the Inspector = max hold duration in seconds
    // e.g. uses = 3 means you can hold for up to 3 seconds total

    bool isDashing;

    // ─────────────────────────────────────────────

    protected override bool IsInUse() => isDashing;

    protected override bool CanPerform() => !state.IsDoingSomething && !isDashing;

    // Called by base on GetKeyDown — starts the dash
    public override void DoUse()
    {
        isDashing = true;

        state.IsDoingSomething = true;
        state.OverrideMovement = true;
        state.SetVerticalVelocity(0f);

        playerAnimator.animator.SetTrigger(dashAnimParam);
    }

    public override void Update()
    {
        base.Update();
        HandleHoldDash();
    }

    void HandleHoldDash()
    {
        if (!isDashing) return;

        float dir = state.IsFacingRight ? 1f : -1f;
        state.SetHorizontalVelocity(dir * dashForce);
        state.SetVerticalVelocity(0f);

        // Drain uses in real time — 1 use = 1 second of dash
        currentUses -= Time.deltaTime;

        // Stop when key released or uses fully drained
        if (!Input.GetKey(AbilityKey) || currentUses <= 0)
            StopDash();
    }

    void StopDash()
    {
        float usesSpent = uses - Mathf.Max(0f, currentUses);
        currentUses = Mathf.Max(0, currentUses);

        isDashing = false;

        state.SetHorizontalVelocity(0f);
        state.IsDoingSomething = false;
        state.OverrideMovement = false;

        StartRefill(usesSpent);
    }
}