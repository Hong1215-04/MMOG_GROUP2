using UnityEngine;

public class P1_SKILL3 : PlayerAbility
{
    [SerializeField] Health health;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float DashForce = 6f;
    [SerializeField] PlayerState P1State;
    [SerializeField] float duration = 1f;
    //[SerializeField] float movementback = 0.5f;
    [SerializeField] GameObject P1Collison;
    [SerializeField] Collider2D col;

    float usedtime;
    public bool Silence;
    bool UsedSkill;

    //bool DashUsed = false;

    protected override void Start()
    {
        base.Start(); // important!
        rb = GetComponent<Rigidbody2D>();
        P1Collison.SetActive(true);
        Silence = false;
        col.enabled = true;
        usedtime = 0f;
        UsedSkill = false;
    }

    public override void Update()
    {
        //if (Time.time - usedtime > movementback)
        //{
        //    col.enabled = true;
        //}
        if (UsedSkill)
        {
            usedtime += Time.deltaTime;
        }

        if (usedtime >= duration)
        {
            health.Not_invincible();
            P1Collison.SetActive(true);
            col.enabled = true;
            UsedSkill = false;
            usedtime = 0f;
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
                UsedSkill = true;  
                usedtime = 0f;
                health.Set_invincible();
                P1Collison.SetActive(false);
                col.enabled = false;
                rb.AddForce(transform.right * DashForce, ForceMode2D.Impulse);
            }
            else if (P1State.IsFacingRight == false)
            {
                UsedSkill = true;
                usedtime = 0f;
                health.Set_invincible();
                P1Collison.SetActive(false);
                col.enabled = false;
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
