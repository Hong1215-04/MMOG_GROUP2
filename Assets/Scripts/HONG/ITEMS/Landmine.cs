using System.Collections;
using UnityEngine;

public class Landmine : Item
{
    [SerializeField] SpriteRenderer LandSprite;
    [SerializeField] GameObject MineTrigger;

    protected override void Start()
    {
        MineTrigger.SetActive(false);
        base.Start();
    }

    public override void DoUse()
    {
        transform.SetParent(null);
        rb.simulated = true;

        LandSprite.enabled = true;
        MineTrigger.SetActive(true);
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        LandSprite.enabled = false;

        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }
    public void MineTriggered()
    {
        MineTrigger.SetActive(false);
        ConsumeUse();
    }

    public void StopDetect()
    {
        rb.simulated = false;
    }
}
