using TMPro;
using UnityEngine;

public class Health_P2 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] private int BaseHealthATK = 2000;
    [SerializeField] private int LoseHP = 28;

    private int _currentHealthATK;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoseLifeATK();
        }
        HealthText.text = _currentHealthATK.ToString();
        if (_currentHealthATK <= 0)
        {
            //play anim if done
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void LoseLifeATK()
    {
        //testinguse
        _currentHealthATK -= LoseHP;

        if (_currentHealthATK <= 0)
        {
            _currentHealthATK = 0;
        }
    }

    public void ResetHealth()
    {
        _currentHealthATK = BaseHealthATK;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("P1_Damage"))
        {
            LoseLifeATK();
        }
    }
}
