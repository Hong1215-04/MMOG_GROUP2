using System.Collections;
using UnityEngine;

public class Wings : Item
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float animDuration = 0.3f;
    public override void DoUse()
    {
        player.GetComponent<PlayerJump>().ForceJump();
        animator.enabled = true;
        animator.applyRootMotion = true;
        animator.SetBool("Fly", true);
        spriteRenderer.enabled = true;
        ConsumeUse();
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        spriteRenderer.enabled = false;
        transform.SetParent(player.GetComponent<PlayerState>().FlapSlot,false);
        transform.localPosition = Vector3.zero;
    }
    protected override void OnUsesCompleted()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(animDuration);
        Destroy(gameObject);
    }
}
