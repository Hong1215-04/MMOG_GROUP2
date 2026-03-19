using UnityEngine;

public class HighJumpSkill : PlayerAbility
{
    [SerializeField] float jumpMultiplyer = 2f;
    [SerializeField] PlayerJump playerjump;

    bool isJump;

    public override void DoUse()
    {
        isJump = true; 
        playerjump.jumpForce = playerjump.jumpForce * jumpMultiplyer ;   
    }

    protected override bool CanPerform()
    {
        return !isJump;
    }

    public override void Update()
    {
        if (isJump == true)
        {
            if (playerjump.jumped == true)
            {
                playerjump.jumpForce = playerjump.jumpForce / jumpMultiplyer;
                isJump = false;
                playerjump.jumped = false;
                ConsumeUse();
            }
        }
     
        base.Update();
    }

}
