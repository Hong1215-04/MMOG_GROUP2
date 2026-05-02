using System;
using UnityEngine;

public class StunOnEnter : MonoBehaviour
{
    [SerializeField] float stunTimer = 3f;
    public Action OnEnterStun;
    public bool stunPlayer = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!stunPlayer) return;
        if(collision.GetComponent<PlayerStun>() != null) 
        {
            collision.GetComponent<PlayerStun>().StunPlayer(stunTimer);
            OnEnterStun?.Invoke();
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
