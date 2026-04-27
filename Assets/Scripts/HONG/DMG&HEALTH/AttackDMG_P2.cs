using UnityEngine;

public class AttackDMG_P2 : MonoBehaviour
{
    public float ATKDMG = 40f;
    [SerializeField] Health healthP1;
    [SerializeField] Health_P2 healthP2;
    [SerializeField] Player_Attack attack;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            healthP1.LoseLifeDEF(ATKDMG);
            healthP2.GainLifeATK(ATKDMG);
            attack.hitted();
        }
    }
}
