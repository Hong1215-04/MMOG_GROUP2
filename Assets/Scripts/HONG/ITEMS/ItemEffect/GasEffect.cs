using UnityEngine;

public class GasEffect : MonoBehaviour
{
    [SerializeField] float GasDuration;
    [SerializeField] float GasDamage;

    private Health PlayerHP;
    float TimeGoes;

    void Start()
    {
        TimeGoes = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        TimeGoes += Time.deltaTime;

        if (TimeGoes >= GasDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHP = other.GetComponent<Health>();

            float GasDmg = GasDamage * 2;
            PlayerHP.LoseLifeDEF(GasDmg);
        }
    }
}
