using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Banana : Item
{
    [SerializeField] StunOnEnter stun;
    [SerializeField] SpriteRenderer spriteRenderer;
    public override void DoUse()
    {
        spriteRenderer.enabled = true;
        transform.SetParent(null);
        rb.simulated = true;
        ConsumeUse();
        StartCoroutine(ShowDetection());
    }
    protected override void Start()
    {
        base.Start();
        stun.stunPlayer = false;
    }

    IEnumerator ShowDetection()
    {
        yield return new WaitForSeconds(0.5f);
        stun.stunPlayer = true;

    }
    protected override void OnUsesCompleted()
    {
    }

    public override void OnPickUp()
    {
        spriteRenderer.enabled = false;
        rb.simulated = false;
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

}
