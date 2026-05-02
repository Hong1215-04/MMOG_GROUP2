using System.Collections;
using UnityEngine;

public class Clock : Item
{

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float timeToAddOrSubtract,animDuration;
    public override void OnPickUp()
    {
        rb.simulated = false;

        spriteRenderer.enabled = false;

        transform.SetParent(player.GetComponent<PlayerState>().HeadSlot);
        transform.localPosition = Vector3.zero;
    }

    public override void DoUse()
    {
        spriteRenderer.enabled = true;
        player.GetComponent<PlayerState>().HeadAnim.SetTrigger("Move");
        if (player.GetComponent<PlayerState>().IsAttacker)
        {
            Timer.Instance.AddTime(timeToAddOrSubtract);

        }
        else 
        {
            Timer.Instance.SubtractTime(timeToAddOrSubtract);
        }
        ConsumeUse();
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
