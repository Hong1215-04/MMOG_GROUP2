using UnityEngine;
using System;

public abstract class Item : UsableBehaviour
{
    public event Action OnItemDestroyed, OnItemUse;
    public PlayerItemInteraction player;
    public bool canPickup = true;
    [SerializeField] protected Rigidbody2D rb;

    protected void InvokeOnItemDestroyed() => OnItemDestroyed?.Invoke();

    protected override void OnUsesCompleted()
    {
        OnItemDestroyed?.Invoke();

        Destroy(gameObject);
    }

    // In Item.cs — add a wrapper that fires the event then calls the implementation
    public void Use()
    {
        OnItemUse?.Invoke();
        DoUse();
    }

    public abstract override void DoUse();
    public abstract void OnPickUp();
}