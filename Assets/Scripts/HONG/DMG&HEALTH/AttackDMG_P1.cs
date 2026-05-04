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
    //private Player_Attack P2ATK;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (healthP1 == null)
        {
            Health[] allHealth = FindObjectsByType<Health>(FindObjectsSortMode.None);
            foreach (Health h in allHealth)
            {
                if (h != healthP2)
                {
                    healthP1 = h;
                    break;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            StateP2 = other.GetComponentInParent<PlayerState>();
            if (healthP1 == null)
            {
                Health[] allHealth = FindObjectsByType<Health>(FindObjectsSortMode.None);
                foreach (Health h in allHealth)
                {
                    if (h != healthP2)
                    {
                        healthP1 = h;
                        break;
                    }
                }
            }
            Debug.Log("DAMAGED");

            if (StateP2.IsBlocked)
            {
                attack.hitted();
            }
            else if (StateP1.IsBuffed)
            {
                //P2ATK.StopAllAtk();
                BuffDmg = ATKDMG * BuffMulti;
                healthP1.LoseLifeDEF(BuffDmg);
                healthP2.GainLifeDEF(BuffDmg);
                attack.hitted();
            }
            else
            {
                //P2ATK.StopAllAtk();
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
