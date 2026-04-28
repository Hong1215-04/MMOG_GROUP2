using UnityEngine;

public class Coins : Item
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public override void DoUse()
    {
        Debug.Log("Got");
        //if (player.GetComponent<PlayerState>())
        //{

        //}
        //else
        //{

        //}
        ConsumeUse();
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        spriteRenderer.enabled = false;
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }
}
