using UnityEngine;

public class Coins : Item
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float CoinPlusHealth;

    private float DEFCoinHealth;
    private float ATKCoinHealth;
    private Health playerhealth;

    public override void DoUse()
    {
        playerhealth = player.GetComponent<Health>();
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
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }
}
