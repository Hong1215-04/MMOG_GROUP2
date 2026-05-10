using UnityEngine;

public class P1_SKILL3 : PlayerAbility
{
    [SerializeField] Health health;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float DashForce = 6f;
    [SerializeField] PlayerState P1State;
    [SerializeField] float duration = 1f;
    [SerializeField] Collider2D col;
    [SerializeField] LayerMask GroundMask;
    //[SerializeField] GameObject P1Collison;

    float usedtime;
    public bool Silence;
    bool dashing;
    bool Hitwall;

    //bool DashUsed = false;

    protected override void Start()
    {
        base.Start(); // important!
        rb = GetComponent<Rigidbody2D>();
        //P1Collison.SetActive(true);
        Silence = false;
    }

    public override void Update()
    {
        float Wide = GetComponentInParent<Collider2D>().bounds.size.x;
        bool HitWall = Physics2D.Raycast((transform.position + new Vector3(0, 0.5f, 0)), Vector2.right, (Wide / 2) + 0.4f, GroundMask);
        bool HitWallLeft = Physics2D.Raycast((transform.position + new Vector3 (0,0.5f,0)), Vector2.left, (Wide / 2) + 0.4f, GroundMask);

        Debug.Log(HitWall);

        if (HitWall || HitWallLeft)
        {
            dashing = false;
            CompleteUse();
            health.Not_invincible();
            //P1Collison.SetActive(true);
            col.enabled = true;
        }

        if (Time.time - usedtime > duration && dashing)
        {
            dashing = false;
            CompleteUse();
            health.Not_invincible();
            //P1Collison.SetActive(true);
            col.enabled = true;
        }
        base.Update();
    }

    public override void DoUse()
    {
            dashing = true;
            Debug.Log("GOT");
            if (P1State.IsFacingRight == true)
            {
                usedtime = Time.time;
                health.Set_invincible();
                //P1Collison.SetActive(false);
                col.enabled = false;
                rb.AddForce(transform.right * DashForce, ForceMode2D.Impulse);
            }
            else if (P1State.IsFacingRight == false)
            {
                usedtime = Time.time;
                health.Set_invincible();
                //P1Collison.SetActive(false);
                col.enabled = false;

                rb.AddForce(-transform.right * DashForce, ForceMode2D.Impulse);
            }
            ConsumeUse();
       
    }

    protected override bool CanPerform()
    {
        return !Silence;
    }
}
