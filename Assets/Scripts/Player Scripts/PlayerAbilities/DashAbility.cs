using UnityEngine;

public class InstantDashAbility : PlayerAbility
{
    [SerializeField] PlayerState state;
    [SerializeField] PlayerAnimator playerAnimator;
    [SerializeField] string dashAnimParam = "Dash";

    [Header("Dash Settings")]
    [SerializeField] float dashForce = 70f;

    protected override bool CanPerform() => !state.IsDoingSomething;

    public override void DoUse()
    {
        float dir = state.IsFacingRight ? 1f : -1f;

        state.IsDoingSomething = true;
        state.OverrideMovement = true;
        state.SetHorizontalVelocity(dir * dashForce);
        state.SetVerticalVelocity(0f);

        playerAnimator.animator.SetTrigger(dashAnimParam);
        ConsumeUse();
        StartRefill(uses - currentUses);
        StartCoroutine(playerAnimator.WaitForAnimationEnd("Dash", OnDashEnd));
    }

    void OnDashEnd()
    {
        state.SetHorizontalVelocity(0f);
        state.IsDoingSomething = false;
        state.OverrideMovement = false;
        CompleteUse();
    }
}