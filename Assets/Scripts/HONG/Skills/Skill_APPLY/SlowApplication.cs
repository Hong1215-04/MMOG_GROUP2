using UnityEngine;

public class SlowApplication : MonoBehaviour
{
    [SerializeField] P2Skill1_SLOW SlowSkill;
    [SerializeField] float SlowSkillMultiplier = 0.8f;
    [SerializeField] float SlowDuration = 3.0f;

    private PlayerState State;
    private PlayerMovement movement;

    float Slowtime;
    bool Slowing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slowing = false;
        Slowtime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Slowtime);
        if (Slowing)
        {
            Slowtime += Time.deltaTime;
        }

        if (Slowtime >= SlowDuration)
        {
            movement.RemoveSpeedMultiplier(SlowSkillMultiplier);
            Slowing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        State = other.GetComponentInParent<PlayerState>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            if (State.IsBlocked)
            {
                SlowSkill.skillhitted();
            }
            else if (State.IsBlocked == false)
            {
                movement = other.GetComponentInParent<PlayerMovement>();
                movement.AddSpeedMultiplier(SlowSkillMultiplier);
                Slowtime = 0f;
                Slowing = true;
                SlowSkill.skillhitted();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        State = other.GetComponentInParent<PlayerState>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            if (State.IsBlocked)
            {
                SlowSkill.skillhitted();
            }
            else if (State.IsBlocked == false)
            {
                movement = other.GetComponentInParent<PlayerMovement>();
                movement.AddSpeedMultiplier(SlowSkillMultiplier);
                Slowtime = 0f;
                Slowing = true;
                SlowSkill.skillhitted();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        State = other.GetComponentInParent<PlayerState>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            if (State.IsBlocked)
            {
                State.IsBlocked = false;
            }
        }    
    }
}
