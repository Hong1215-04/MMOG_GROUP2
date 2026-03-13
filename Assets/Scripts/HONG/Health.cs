using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] private int BaseHealthDef = 8000;
    [SerializeField] private int LoseHP = 1000;

    private int _currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ResetHealth();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LoseLife();
        }
        HealthText.text = _currentHealth.ToString();
        if (_currentHealth <= 0)
        {
            //play anim if done
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void LoseLife()
    {
        //testinguse
        _currentHealth -= LoseHP;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
        }
    }

    public void ResetHealth()
    {
        _currentHealth = BaseHealthDef;
    }
}
