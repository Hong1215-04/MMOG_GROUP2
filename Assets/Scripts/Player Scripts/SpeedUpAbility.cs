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
        if (!Silence)
        {
            playerMovement.AddSpeedMultiplier(speedMultiplier);
            usedTime = Time.time;
            isSpeed = true;
            ConsumeUse();
            StartRefill(uses - currentUses);
        }
        if (Silence)
        {
            Debug.Log("GOTSIL");
        }
    }

    protected override bool CanPerform()
    {
        return !isSpeed;
    }

    public override void Update()
    {
        if (Time.time - usedTime > duration)
        {
            playerMovement.RemoveSpeedMultiplier(speedMultiplier);
            isSpeed = false;
        }
        base.Update();
    }
}
