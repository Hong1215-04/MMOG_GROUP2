using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class Health_P2 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] private float BaseHealthAtk = 2000f;
    //public float LoseHP = 40f;
    [SerializeField] PlayerMovement P2Movement;
    [SerializeField] PlayerJump P2Jump;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float recovertime = 0.8f;
    public Action OnDamageTaken;
    //Vector2 lastdirection;
    private float _currentHealthATK;

    //bool invincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetHealth();
        //invincible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    LoseLifeDEF();
        //}
        HealthText.text = _currentHealthATK.ToString();

        if (_currentHealthATK <= 0)
        {
            //play anim if done
            UnityEditor.EditorApplication.isPlaying = false;
        }
        //lastdirection = rb.linearVelocity;
    }

    public void LoseLifeATK(float LoseHP)
    {
        //testinguse
        _currentHealthATK -= LoseHP / 2;

        if (_currentHealthATK <= 0)
        {
            _currentHealthATK = 0;
        }
    }

    public void GainLifeATK(float LoseHP)
    {
        _currentHealthATK += LoseHP / 2;
    }

    public void ResetHealth()
    {
        _currentHealthATK = BaseHealthAtk;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("P1_Damage"))
        {
            //P2Movement.CannotMove();
            //P2Jump.CannotJump();
            //StartCoroutine(BeingHit());
            //OnDamageTaken?.Invoke();

            P2Movement.CannotMove();
            P2Jump.CannotJump();

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

        //if (other.gameObject.layer == LayerMask.NameToLayer("Taser"))
        //{
        //    P2Movement.CannotMove();
        //    P2Jump.CannotJump();
        //    StartCoroutine(Tased());
        //}
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("P1_Damage"))
        {

            P2Movement.CannotMove();
            P2Jump.CannotJump();

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
    //    yield return new WaitForSeconds(0.8f);
    //    P2Movement.CanMove();
    //    P2Jump.CanJump();
    //}

    IEnumerator RegainMove()
    {
        yield return new WaitForSeconds(recovertime);
        P2Movement.CanMove();
        P2Jump.CanJump();
    }

    //IEnumerator Tased()
    //{
    //    rb.linearVelocity = Vector2.zero;
    //    yield return new WaitForSeconds(0.75f);
    //    P2Movement.CanMove();
    //    P2Jump.CanJump();
    //}

    //public void set_invincible()
    //{
    //    invincible = true;
    //}

    //public void not_invincible()
    //{
    //    invincible = false;
    //}
}
