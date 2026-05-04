using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    PlayerState state;
    float stunDuration, stunnedTime;
    [SerializeField] GameObject spinningStarsGO;
    Animator animator;

    private void Start()
    {
        state = GetComponent<PlayerState>();
        animator = GetComponent<Animator>();
    }

    public void StunPlayer(float duration)
    {
        spinningStarsGO.SetActive(true);
        state.IsStunned = true;
        animator.SetBool("Stun",true);
        state.IsDoingSomething = true;

        float remaining = stunDuration - (Time.time - stunnedTime);
        if (duration > remaining)
        {
            stunnedTime = Time.time;
            stunDuration = duration;
        }
        // else: current stun still has more time, do nothing
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    StunPlayer(3f);
        //}
        if (state.IsStunned) 
        {
            if (Time.time - stunnedTime > stunDuration) 
            {
                ExitStun();
            }
        }
    }


    public void ExitStun()
    {
        state.IsStunned=false;
        spinningStarsGO.SetActive(false);
        animator.SetBool("Stun", false);
        state.IsDoingSomething = false;
    }
}
