using System;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    Item heldItem;
    [SerializeField] KeyCode useItemKey = KeyCode.C;
    public Action OnItemPickup;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (heldItem != null) return; // already holding something, ignore

        Item item = other.GetComponent<Item>();
        if (item != null && item.canPickup)
        {
            heldItem = item;
            heldItem.player = this;
            heldItem.canPickup = false;
            heldItem.OnItemDestroyed += OnHeldItemDestroyed;
            heldItem.OnPickUp();
            OnItemPickup?.Invoke();
            ItemSpawner.Instance.numItemsInMap--;
        }
    }

    public void SetHeldItem(Item item)
    {
        heldItem = item;
    }

    void OnHeldItemDestroyed()
    {
        heldItem.OnItemDestroyed -= OnHeldItemDestroyed;
        heldItem.player = null;
        heldItem = null;
    }

    void Update()
    {
        if (heldItem != null && Input.GetKeyDown(useItemKey))
        {
            heldItem.Use();
        }
    }
}