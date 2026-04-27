using UnityEngine;

public class AttackDMG : MonoBehaviour
{
    public float ATKDMG = 40f;
    [SerializeField] Health_P2 healthP2;
    [SerializeField] Health healthP1;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            healthP2.LoseLifeATK(ATKDMG);
            healthP1.GainLifeDEF(ATKDMG);
            attack.hitted();
        }
    }
}
