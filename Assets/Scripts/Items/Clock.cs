using UnityEngine;

public class Clock : Item
{

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float timeToAddOrSubtract;
    public override void OnPickUp()
    {
        rb.simulated = false;

        spriteRenderer.enabled = false;

        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

    public override void DoUse()
    {
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
}
