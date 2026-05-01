using UnityEngine;

public class AttackDMG : MonoBehaviour
{
    public float ATKDMG = 40f;
    [SerializeField] Health healthP2;
    [SerializeField] Health healthP1;
    [SerializeField] Player_Attack attack;
    [SerializeField] PlayerState StateP1;
    [SerializeField] float BuffMulti = 1.5f;

    private PlayerState StateP2;
    private float BuffDmg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            StateP2 = other.GetComponentInParent<PlayerState>();

            if (StateP2.IsBlocked)
            {
                attack.hitted();
            }
            else if (StateP1.IsBuffed)
            {
                BuffDmg = ATKDMG * BuffMulti;
                healthP1.LoseLifeDEF(BuffDmg);
                healthP2.GainLifeDEF(BuffDmg);
                attack.hitted();
            }
            else
            {
                healthP1.LoseLifeDEF(ATKDMG);
                healthP2.GainLifeDEF(ATKDMG);
                attack.hitted();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        StateP2 = other.GetComponentInParent<PlayerState>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            if (StateP2.IsBlocked)
            {
                StateP2.IsBlocked = false;
            }
        }
    }
}
