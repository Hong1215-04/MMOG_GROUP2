using TMPro;
using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] private float BaseHealthDef = 8000f;
    //public float LoseHP = 40f;
    [SerializeField] PlayerMovement P1Movement;
    [SerializeField] PlayerJump P1Jump;
    [SerializeField] Rigidbody2D rb ;
    public Action OnDamageTaken;
    Vector2 lastdirection;
    private float _currentHealthDEF;

    bool invincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetHealth();
        invincible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    LoseLifeDEF();
        //}
        HealthText.text = _currentHealthDEF.ToString();

        if (_currentHealthDEF <= 0)
        {
            //play anim if done
            UnityEditor.EditorApplication.isPlaying = false;
        }
        lastdirection = rb.linearVelocity;
    }

    public void LoseLifeDEF(float LoseHP)
    {
        //testinguse
        _currentHealthDEF -= LoseHP / 2;

        if (_currentHealthDEF <= 0)
        {
            _currentHealthDEF = 0;
        }
    }

    public void GainLifeDEF(float LoseHP)
    {
        _currentHealthDEF += LoseHP / 2;
    }

    public void ResetHealth()
    {
        _currentHealthDEF = BaseHealthDef;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("P2_Damage"))
        {
            if (invincible == false)
            {
                P1Movement.CannotMove();
                P1Jump.CannotJump();
                StartCoroutine(BeingHit());
                OnDamageTaken?.Invoke();
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Taser"))
        {
            P1Movement.CannotMove();
            P1Jump.CannotJump();
            StartCoroutine(Tased());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("P2_Damage"))
        {

        }
    }

    IEnumerator BeingHit()
    {
        Vector2 IncomingDir = lastdirection.normalized;
        Vector2 Knockback = -IncomingDir;
        rb.AddForce(Knockback * 6f, ForceMode2D.Impulse);
        //rb.linearVelocity = -lastdirection;
        yield return new WaitForSeconds(0.8f);
        P1Movement.CanMove();
        P1Jump.CanJump();
    }

    IEnumerator Tased()
    {
        rb.linearVelocity = Vector2.zero;   
        yield return new WaitForSeconds(0.75f);
        P1Movement.CanMove();
        P1Jump.CanJump();
    }

    public void set_invincible()
    {
        invincible = true;
    }

    public void not_invincible()
    {
        invincible = false;
    }
}
