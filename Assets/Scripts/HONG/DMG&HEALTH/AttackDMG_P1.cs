using UnityEngine;

public class AttackDMG : MonoBehaviour
{
    public float ATKDMG = 40f;
    [SerializeField] Health healthP2;
    [SerializeField] Health healthP1;
    [SerializeField] Player_Attack attack;
    [SerializeField] PlayerState State;
    [SerializeField] float BuffMulti = 1.5f;

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
            if (State.IsBuffed)
            {
                BuffDmg = ATKDMG * BuffMulti;
                healthP2.LoseLifeDEF(BuffDmg);
                healthP1.GainLifeDEF(BuffDmg);
                attack.hitted();
            }
            else
            {
                healthP2.LoseLifeDEF(ATKDMG);
                healthP1.GainLifeDEF(ATKDMG);
                attack.hitted();
            }
        }
    }
}
