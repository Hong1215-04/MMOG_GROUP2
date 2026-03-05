using UnityEngine;
using System;

public abstract class Item : UsableBehaviour
{
    public event Action OnItemDestroyed;
    public PlayerItemInteraction player;
    public bool canPickup = true;
    [SerializeField] protected Rigidbody2D rb;

    protected override void OnUsesCompleted()
    {
        OnItemDestroyed?.Invoke();

        Destroy(gameObject);
    }

    protected abstract override void DoUse();
    public abstract void OnPickUp();
}