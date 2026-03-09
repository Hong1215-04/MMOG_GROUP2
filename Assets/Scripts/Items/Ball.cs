using UnityEngine;

public class Ball : Item
{
    protected override void DoUse()
    {
        Debug.Log("THROWW BALL");
        ConsumeUse();
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }
}
