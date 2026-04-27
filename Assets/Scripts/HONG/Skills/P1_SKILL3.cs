using UnityEngine;

public class P1_SKILL3 : PlayerAbility
{
    [SerializeField] Health health;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float DashForce = 6f;
    [SerializeField] PlayerState P1State;
    [SerializeField] float duration = 1f;

    float usedtime;
    public bool Silence;

    //bool DashUsed = false;

    protected override void Start()
    {
        base.Start(); // important!
        rb = GetComponent<Rigidbody2D>();
        Silence = false;
    }

    public override void Update()
    {
        if (Time.time - usedtime > duration)
        {
            health.not_invincible();
        }
        base.Update();
    }

    public override void DoUse()
    {
        if (!Silence)
        {
            Debug.Log("GOT");
            if (P1State.IsFacingRight == true)
            {
                usedtime = Time.time;
                health.set_invincible();
                rb.AddForce(transform.right * DashForce, ForceMode2D.Impulse);
            }
            else if (P1State.IsFacingRight == false)
            {
                usedtime = Time.time;
                health.set_invincible();
                rb.AddForce(-transform.right * DashForce, ForceMode2D.Impulse);
            }
            ConsumeUse();
        }
        //Vector2 playerdir;
        //playerdir = rb.linearVelocity;
        //float facedir = playerdir.normalized.x;
        //Vector2 Dashdir = new Vector2(facedir, 0);
    }

    protected override bool CanPerform()
    {
        return true;
    }
}
