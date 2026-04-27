using UnityEngine;

public class SlowApplication : MonoBehaviour
{
    [SerializeField] P2Skill1_SLOW SlowSkill;
    [SerializeField] float SlowSkillMultiplier = 0.8f;
    [SerializeField] float SlowDuration = 3.0f;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            movement = other.GetComponent<PlayerMovement>();
            movement.AddSpeedMultiplier(SlowSkillMultiplier);
            Slowtime = 0f;
            Slowing = true;
            SlowSkill.skillhitted();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            movement = other.GetComponent<PlayerMovement>();
            movement.AddSpeedMultiplier(SlowSkillMultiplier);
            Slowtime = 0f;
            Slowing = true; 
            SlowSkill.skillhitted();
        }
    }
}
