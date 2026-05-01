using System.Collections;
using UnityEngine;

public class MineStun : MonoBehaviour
{
    [SerializeField] float MineBlowDistance = 10f;
    [SerializeField] float MineDamage = 200f;
    [SerializeField] Landmine Mine;
    [SerializeField] float StunTime = 1.2f;
    
    private Health PlayerHealth;
    private PlayerMovement Movement;
    private PlayerJump Jump;
    private Rigidbody2D playerrb;
    public bool Activated = false;
    public bool IsInvincible;

    private void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Activated)
            {
                PlayerHealth = other.GetComponentInParent<Health>();

                Movement = other.GetComponentInParent<PlayerMovement>();
                Jump = other.GetComponentInParent<PlayerJump>();
                playerrb = other.GetComponentInParent<Rigidbody2D>();

                Movement.CannotMove();
                Jump.CannotJump();

                Vector2 playerPos = other.transform.position;
                Vector2 minePos = transform.position;

                Vector2 direction = (playerPos - minePos).normalized;
                float distance = Vector2.Distance(playerPos, minePos);

                float maximumdis = 3.0f;
                float normalizeddis = Mathf.Clamp01(distance / maximumdis);

                float inverted = 1f - normalizeddis; //closer -- bigger value (more close -- normalizedis more small)

                float force = Mathf.Lerp(5f, MineBlowDistance, inverted);

                //rb.linearVelocity = Vector2.zero;
                playerrb.AddForce(direction * force, ForceMode2D.Impulse);

                float minedmg = MineDamage * 2f;
                PlayerHealth.LoseLifeDEF(minedmg);

                StartCoroutine(RegainMove_Del());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Activated = true;
        }
    }

    IEnumerator RegainMove_Del()
    {
        Mine.StopDetect();
        yield return new WaitForSeconds(StunTime);
        Movement.CanMove();
        Jump.CanJump();
        Activated = false;
        Mine.MineTriggered();
    }
}
