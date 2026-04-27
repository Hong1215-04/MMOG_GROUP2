using System.Collections;
using UnityEngine;

/// <summary>
/// PlayerAnimator — Animation Driver
///
/// Reads PlayerState and pushes values into the Animator.
/// No game logic lives here.
///
/// CURRENT PARAMETERS (must exist in your Animator Controller):
///   Bool : "InAir"          — true when not grounded (jumping, falling)
///   Bool : "IsWalking"      — true when grounded and moving horizontally
///   Bool : "IsDoingSomething" — true while any ability is active
/// </summary>
[RequireComponent(typeof(PlayerState))]
public class PlayerAnimator : MonoBehaviour
{
    const string PARAM_IN_AIR = "InAir";
    const string PARAM_IS_WALKING = "IsWalking";
    const string PARAM_IS_DOING_SOMETHING = "IsDoingSomething";

    PlayerState state;
    public Animator animator;

    void Awake()
    {
        state = GetComponent<PlayerState>();
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogWarning("PlayerAnimator: No Animator found on this GameObject or its children.");
    }

    void Update()
    {
        if (animator == null) return;

        // While an ability owns the animator, only drive IsDoingSomething
        // and leave the ability's trigger/state alone.
        animator.SetBool(PARAM_IS_DOING_SOMETHING, state.IsDoingSomething);

        if (state.IsDoingSomething) return;

        animator.SetBool(PARAM_IN_AIR, !state.IsGrounded);
        animator.SetBool(PARAM_IS_WALKING, state.IsGrounded && Mathf.Abs(state.MoveInput) > 0.01f);
    }

    /// <summary>
    /// Waits for the Animator to enter then finish a named state.
    /// A timeout prevents OverrideMovement getting stuck permanently
    /// if the trigger is missed or the state name doesn't match.
    /// </summary>
    public IEnumerator WaitForAnimationEnd(string stateName, System.Action onComplete, float timeout = 2f)
    {
        float elapsed = 0f;

        // Wait up to timeout for the named state to become active.
        // Using deltaTime accumulation instead of WaitForSeconds so we
        // can also check the state name each frame.
        yield return new WaitUntil(() =>
        {
            elapsed += Time.deltaTime;
            return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)
                   || elapsed >= timeout;
        });

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            // State is active — wait for it to reach the end
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        }
        else
        {
            Debug.LogWarning($"PlayerAnimator: Timed out waiting for state '{stateName}'. " +
                             "Verify the name matches exactly in the Animator Controller " +
                             "and that the transition has 'Has Exit Time' unchecked.");
        }

        onComplete?.Invoke();
    }
}