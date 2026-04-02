using UnityEngine;

public class P1_SKILL3 : PlayerAbility
{
    [SerializeField] GameObject PlayerCollision;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float DashForce = 6f;

    bool DashUsed = false;

    public override void Update()
    {

        base.Update();
    }

    public override void DoUse()
    {
        Debug.Log("GOT");
        rb = GetComponent<Rigidbody2D>();
        Vector2 playerdir;
        playerdir = rb.linearVelocity;
        float facedir = playerdir.normalized.x;
        Vector2 Dashdir = new Vector2(facedir, 0);
        rb.AddForce(Dashdir * DashForce, ForceMode2D.Impulse);
        ConsumeUse();
    }

    protected override bool CanPerform()
    {
        //ConsumeUse();
        throw new System.NotImplementedException();
    }
}
