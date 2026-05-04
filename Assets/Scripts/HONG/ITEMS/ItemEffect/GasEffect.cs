using UnityEngine;

public class GasEffect : MonoBehaviour
{
    [SerializeField] float GasDuration;
    [SerializeField] float GasDamage;
    [SerializeField] float SlowSkillMultiplier;

    private PlayerMovement Pmovement;
    private Health PlayerHP;
    float TimeGoes;
    bool Slowed;

    void Start()
    {
        Slowed = false;
        TimeGoes = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        TimeGoes += Time.deltaTime;

        if (TimeGoes >= GasDuration)
        {
            if (Pmovement != null)
            {
                Pmovement.RemoveSpeedMultiplier(SlowSkillMultiplier);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHP = other.GetComponentInParent<Health>();
            Pmovement = other.GetComponentInParent<PlayerMovement>();

            if (PlayerHP.IsAttacker)
            {
                float GasDmg = GasDamage * 1.2f;
                PlayerHP.LoseLifeDEF(GasDmg);
                if (!Slowed)
                {
                    Pmovement.AddSpeedMultiplier(SlowSkillMultiplier);
                    Slowed = true;
                }
            }
            else
            {
                float GasDmg = GasDamage * 2;
                PlayerHP.LoseLifeDEF(GasDmg);
                if (!Slowed)
                {
                    Pmovement.AddSpeedMultiplier(SlowSkillMultiplier);
                    Slowed = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Pmovement = other.GetComponentInParent<PlayerMovement>();

        if (Slowed)
        {
            Pmovement.RemoveSpeedMultiplier(SlowSkillMultiplier);
            Slowed = false;
        }
    }
}
