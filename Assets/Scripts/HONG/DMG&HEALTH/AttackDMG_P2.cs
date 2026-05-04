using System.Collections;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class AttackDMG_P2 : MonoBehaviour
{
    public float ATKDMG = 40f;
    [SerializeField] Health healthP1;
    [SerializeField] Health healthP2;
    [SerializeField] Player_Attack attack;
    [SerializeField] PlayerState StateP2;
    [SerializeField] float BuffMulti = 1.5f;
    private PlayerState StateP1;
    private float BuffDmg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(healthP1 == null)
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
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
            Debug.Log("DAMAGED");
            StateP1 = other.GetComponentInParent<PlayerState>();

            if (StateP1.IsBlocked)
            {
                attack.hitted();
            }
            else if (StateP2.IsBuffed)
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
        StateP1 = other.GetComponentInParent<PlayerState>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            if (StateP1.IsBlocked)
            {
                StateP1.IsBlocked = false;
            }
        }
    }
}
