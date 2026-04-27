using UnityEngine;

public class P2_SKILL2 : PlayerAbility
{
    [SerializeField] PlayerState PlayerState;
    [SerializeField] Taser taserprefab;
    [SerializeField] Transform bulletspawnPos;

    public override void DoUse()
    {
        if (PlayerState.IsFacingRight == true)
        {
            Taser taser = Instantiate(taserprefab, bulletspawnPos.position, Quaternion.identity);
            taser.ShootRight();
        }
        else if (PlayerState.IsFacingRight == false)
        {
            Taser taser = Instantiate(taserprefab, bulletspawnPos.position, Quaternion.identity);
            taser.ShootLeft();
        }
        ConsumeUse();
    }

    protected override bool CanPerform()
    {
        return true;
    }

    public override void Update()
    {

        base.Update();
    }
}
