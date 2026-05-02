using UnityEngine;

public class SpeedUpAbility : PlayerAbility
{
    [SerializeField] float duration;
    [SerializeField] float speedMultiplier;
    [SerializeField] PlayerMovement playerMovement;
    float usedTime;

    bool isSpeed;
    public bool Silence;

    protected override void Start()
    {
        base.Start(); // important!
        Silence = false;
    }

    public override void DoUse()
    {
        playerMovement.AddSpeedMultiplier(speedMultiplier);
        usedTime = Time.time;
        isSpeed = true;
        ConsumeUse();
        StartRefill(uses - currentUses);
    }

    protected override bool CanPerform()
    {
        return !isSpeed && !Silence;
    }

    public override void Update()
    {
        if (Time.time - usedTime > duration && isSpeed)
        {
            CompleteUse();
            playerMovement.RemoveSpeedMultiplier(speedMultiplier);
            isSpeed = false;
        }
        base.Update();
    }
}
