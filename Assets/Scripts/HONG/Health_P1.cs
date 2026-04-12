using TMPro;
using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] private int BaseHealthDef = 8000;
    [SerializeField] private int LoseHP = 40;
    [SerializeField] PlayerMovement P1Movement;
    [SerializeField] PlayerJump P1Jump;
    [SerializeField] Rigidbody2D rb ;
    public Action OnDamageTaken;
    Vector2 lastdirection;
    private int _currentHealthDEF;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        lastdirection = rb.linearVelocity;
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
            P1Movement.CannotMove();
            P1Jump.CannotJump();
            StartCoroutine(BeingHit());
            OnDamageTaken?.Invoke();
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
}
