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
    [SerializeField] PlayerStun StunEffect;
    [SerializeField] PlayerJump P1Jump;
    [SerializeField] Rigidbody2D rb ;
    [SerializeField] float recovertime = 2f;
    [SerializeField] float Tasedtime = 1.0f;
    public Action OnDamageTaken;
    [SerializeField] String layer;
    //Vector2 lastdirection;
    private float _currentHealthDEF;
    public bool IsAttacker;

    public bool invincible = false;

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
        //lastdirection = rb.linearVelocity;
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
        if (other.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            if (invincible == false)
            {
                //P1Movement.CannotMove();
                //P1Jump.CannotJump();
                //StartCoroutine(BeingHit());
                OnDamageTaken?.Invoke();

                P1Movement.CannotMove();
                P1Jump.CannotJump();

                Vector2 enemyPos = other.transform.parent.position;
                Vector2 playerPos = transform.position;

                Vector2 direction = (playerPos - enemyPos).normalized;
                float distance = Vector2.Distance(playerPos, enemyPos);

                float maximumdis = 5.0f;
                float normalizeddis = Mathf.Clamp01(distance / maximumdis);

                float inverted = 1f - normalizeddis; //closer -- bigger value (more close -- normalizedis more small)

                float force = Mathf.Lerp(5.5f, 12f, inverted);

                //rb.linearVelocity = Vector2.zero;
                rb.AddForce(direction * force, ForceMode2D.Impulse);

                StartCoroutine(RegainMove());
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
        if (other.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            P1Movement.CannotMove();
            P1Jump.CannotJump();

            Vector2 enemyPos = other.transform.parent.position;
            Vector2 playerPos = transform.position;

            Vector2 direction = (playerPos - enemyPos).normalized;
            float distance = Vector2.Distance(playerPos, enemyPos);

            float maximumdis = 5.0f;
            float normalizeddis = Mathf.Clamp01(distance / maximumdis);

            float inverted = 1f - normalizeddis; //closer -- bigger value (more close -- normalizedis more small)

            float force = Mathf.Lerp(5.5f, 12f, inverted);

            rb.AddForce(direction * force, ForceMode2D.Impulse);

            StartCoroutine(RegainMove());
        }
    }

    //IEnumerator BeingHit()
    //{
    //    Vector2 IncomingDir = lastdirection.normalized;
    //    Vector2 Knockback = -IncomingDir;
    //    rb.AddForce(Knockback * 6f, ForceMode2D.Impulse);
    //    //rb.linearVelocity = -lastdirection;
    //    yield return new WaitForSeconds(recovertime);
    //    P1Movement.CanMove();
    //    P1Jump.CanJump();
    //}

    IEnumerator RegainMove()
    {
        yield return new WaitForSeconds(recovertime);
        P1Movement.CanMove();
        P1Jump.CanJump();
    }

    IEnumerator Tased()
    {
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(Tasedtime);
        P1Movement.CanMove();
        P1Jump.CanJump();
        //StunEffect.StunPlayer(Tasedtime);
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
