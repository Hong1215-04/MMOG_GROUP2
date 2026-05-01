using UnityEngine;

public class Bubble : Item
{
    [SerializeField] SpriteRenderer BubbleRen;

    private Health PlayerHealth;
    private PlayerState State;
    bool Affecting = false;
    bool BlockedATK = false;
    private float waittime = 0f;
    private float waitingtime = 0.1f;

    public override void DoUse()
    {
        State = player.GetComponent<PlayerState>();
        PlayerHealth = player.GetComponent<Health>();
        PlayerHealth.Set_invincible();
        State.IsBlocked = true;
        Affecting = true;
        BubbleRen.enabled = true;
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        BubbleRen.enabled = false;
        transform.SetParent(player.transform);
        transform.localPosition = new Vector3 (0,0.5f,0);
    }

    private void Update()
    {
        if (Affecting)
        {
            if (State.IsBlocked == false)
            {
                BlockedATK = true;
            }
        }

        if (BlockedATK)
        {
            waittime += Time.deltaTime;
        }

        if (waittime >= waitingtime)
        {
            BlockedATK = false;
            PlayerHealth.Not_invincible();
            ConsumeUse();
        }
    }
}
