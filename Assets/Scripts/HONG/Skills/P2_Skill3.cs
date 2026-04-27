using UnityEngine;

public class P2_Skill3 : PlayerAbility
{
    [SerializeField] GameObject Player1;
    [SerializeField] float EffectRange = 10f;
    [SerializeField] float EffectDuration = 3f;
    [SerializeField] float SlowSkillMultiplier = 0.8f;
    [SerializeField] P1_SKILL3 P1Skill3;
    [SerializeField] SpeedUpAbility P1Skill1;
    [SerializeField] HighJumpSkill P1Skill2;

    

    Vector2 EnemyPos;
    Vector2 P2_Pos;
    float distance;
    float time;
    private PlayerMovement movement;

    bool Casting;
    bool CanCast;
    public override void DoUse()
    {
        if (CanCast)
        {
            time = 0;
            P1Skill1.Silence = true;
            P1Skill2.Silence = true;
            P1Skill3.Silence = true;
            movement = Player1.GetComponent<PlayerMovement>();
            movement.AddSpeedMultiplier(SlowSkillMultiplier);
            Casting = true;
        }  
    }

    protected override bool CanPerform()
    {
        return true;
    }

    public override void Update()
    {
        EnemyPos = Player1.transform.position;
        P2_Pos = transform.position;
        distance = Vector2.Distance(EnemyPos, P2_Pos);

        if (distance <= EffectRange)
        {
            CanCast = true;
        }
        else
        {
            CanCast = false;
        }

        if (Casting)
        {
            time += Time.deltaTime;
        }

        if (time >= EffectDuration)
        {
            Casting = false;
            P1Skill1.Silence = false;
            P1Skill2.Silence = false;
            P1Skill3.Silence = false;
            movement.RemoveSpeedMultiplier(SlowSkillMultiplier);
        }
        base.Update();
    }

}
