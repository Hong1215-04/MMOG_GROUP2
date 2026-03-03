using UnityEngine;

/// <summary>
/// PlayerAnimator — Animation Driver
///
/// Reads PlayerState and pushes values into the Animator.
/// No game logic lives here.
///
/// CURRENT PARAMETERS (must exist in your Animator Controller):
///   Bool : "InAir"     — true when not grounded (jumping, falling)
///   Bool : "IsWalking" — true when grounded and moving horizontally
///
/// HOW TO ADD A NEW ABILITY ANIMATION:
///   1. Add a Bool parameter to the Animator Controller.
///   2. Uncomment or add: animator.SetBool("MyAbility", state.IsMyAbility);
///   3. Wire the transition in the Animator. Done.
/// </summary>
[RequireComponent(typeof(PlayerState))]
public class PlayerAnimator : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Animator parameter name constants
    //  Change these if your parameter names differ
    // ─────────────────────────────────────────────
    const string PARAM_IN_AIR = "InAir";
    const string PARAM_IS_WALKING = "IsWalking";

    // ─────────────────────────────────────────────
    PlayerState state;
    Animator animator;

    void Awake()
    {
        state = GetComponent<PlayerState>();
        // Check this GameObject first, then children (covers both setups)
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogWarning("PlayerAnimator: No Animator found on this GameObject or its children.");
    }

    void Update()
    {
        if (animator == null) return;

        // InAir — true whenever the player is not grounded
        animator.SetBool(PARAM_IN_AIR, !state.IsGrounded);
        // IsWalking — true when grounded and actively moving horizontally
        animator.SetBool(PARAM_IS_WALKING, state.IsGrounded && Mathf.Abs(state.MoveInput) > 0.01f);

        // ── When you add Dash / Roll, uncomment these:
        // animator.SetBool("IsDashing", state.IsDashing);
        // animator.SetBool("IsRolling", state.IsRolling);
    }
}