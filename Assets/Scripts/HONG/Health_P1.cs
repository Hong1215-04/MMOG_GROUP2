using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] private int BaseHealthDef = 8000;
    [SerializeField] private int LoseHP = 40;

    private int _currentHealthDEF;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ResetHealth();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoseLifeDEF();
        }
        HealthText.text = _currentHealthDEF.ToString();
        if (_currentHealthDEF <= 0)
        {
            //play anim if done
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void LoseLifeDEF()
    {
        //testinguse
        _currentHealthDEF -= LoseHP;

        if (_currentHealthDEF <= 0)
        {
            _currentHealthDEF = 0;
        }
    }

    public void ResetHealth()
    {
        _currentHealthDEF = BaseHealthDef;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("P2_Damage"))
        {
            LoseLifeDEF();
        }
    }
}
