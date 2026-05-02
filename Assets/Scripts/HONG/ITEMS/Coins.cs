using System.Collections;
using UnityEngine;

public class Coins : Item
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float CoinPlusHealth, animDuration;
    

    private float DEFCoinHealth;
    private float ATKCoinHealth;
    private Health playerhealth;

    public override void DoUse()
    {
        spriteRenderer.enabled = true;
        playerhealth = player.GetComponentInChildren<Health>();
        player.GetComponent<PlayerState>().HeadAnim.SetTrigger("Move");
        if (playerhealth.IsAttacker)
        {
            ATKCoinHealth = CoinPlusHealth * 2f;
            playerhealth.GainLifeDEF(ATKCoinHealth);
            ConsumeUse();
        }
        else if (!playerhealth.IsAttacker)
        {
            DEFCoinHealth = CoinPlusHealth * 1.5f;
            playerhealth.GainLifeDEF(DEFCoinHealth);
            ConsumeUse();
        }
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        spriteRenderer.enabled = false;
        transform.SetParent(player.GetComponent<PlayerState>().HeadSlot);
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
