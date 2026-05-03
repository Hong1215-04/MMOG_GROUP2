using UnityEngine;

public class Boxing : Item
{
    [SerializeField] SpriteRenderer BoxingRen;

    private Player_Attack Attack;
    private PlayerState State;
    bool Affecting = false;

    public override void DoUse()
    {
        State = player.GetComponent<PlayerState>();
        Attack = player.GetComponent<Player_Attack>();
        State.IsBuffed = true;
        Affecting = true;
        BoxingRen.enabled = true;
        Attack.buffed();
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        BoxingRen.enabled = false;
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (Affecting)
        {
            if (State.IsBuffed == false)
            {
                ConsumeUse();
            }
        }
    }
}
