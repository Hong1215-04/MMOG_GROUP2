using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    Item heldItem;
    [SerializeField] KeyCode useItemKey = KeyCode.C;

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
        }
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
            heldItem.ConsumeUse();
        }
    }
}