using UnityEngine;

public class HighJumpSkill : PlayerAbility
{
    [SerializeField] float jumpMultiplyer = 2f;
    [SerializeField] PlayerJump playerjump;

    //bool isJump;
    bool jumpMultiplierAdded;

    public override void DoUse()
    { 
        playerjump.jumpForce = playerjump.jumpForce * jumpMultiplyer ;
        ConsumeUse();
        jumpMultiplierAdded = true;
    }

    private void OnEnable()
    {
        playerjump.onJumped += Jumped;
    }

    void Jumped() 
    {
        if (jumpMultiplierAdded)
        {
            playerjump.jumpForce = playerjump.jumpForce / jumpMultiplyer;
            jumpMultiplierAdded = false;
        }
    }

    protected override bool CanPerform()
    {
        return !jumpMultiplierAdded;
    }

    public override void Update()
    {
        //if (isJump == true)
        //{
        //    if (playerjump.jumped == true)
        //    {
        //        playerjump.jumpForce = playerjump.jumpForce / jumpMultiplyer;
        //        isJump = false;
        //        playerjump.jumped = false;
        //        ConsumeUse();
        //    }
        //}
     
        base.Update();
    }

}
