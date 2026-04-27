using UnityEngine;

public class P2_Skill3 : PlayerAbility
{
    [SerializeField] GameObject Player1;

    bool CanCast;
    public override void DoUse()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanPerform()
    {
        return true;
    }


}
