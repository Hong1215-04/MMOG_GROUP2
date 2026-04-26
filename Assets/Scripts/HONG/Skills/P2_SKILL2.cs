using UnityEngine;

public class P2_SKILL2 : PlayerAbility
{
    [SerializeField] PlayerState PlayerState;

    public override void DoUse()
    {

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
