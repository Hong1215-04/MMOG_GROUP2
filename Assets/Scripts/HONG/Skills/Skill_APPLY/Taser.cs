using UnityEngine;
using System.Collections;

public class Taser : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float bulletspeed = 30.0f;
    int life = 1;

    private PlayerState State;
    //float avaliabletime;
    //float flytime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootLeft()
    {
        Debug.Log("Got");
        rb.linearVelocity = -Vector2.right * bulletspeed;
    }

    public void ShootRight()
    {
        rb.linearVelocity = Vector2.right * bulletspeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            State = other.gameObject.GetComponentInParent<PlayerState>();

            if (State.IsBlocked)
            {
                State.IsBlocked = false;

                life--;
                if (life <= 0)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            else
            {
                life--;
                if (life <= 0)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
        //other.gameObject.GetComponentInParent<PlayerStun>().StunPlayer(1f);
        else
        {
            life--;
            if (life <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
