using System;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    Item heldItem;
    public KeyCode useItemKey = KeyCode.C;
    public Action OnItemPickup;
    [SerializeField] GameObject inventorySlot;
    [SerializeField] SpriteRenderer inventoryItemImage;

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

        if (heldItem != null)
        {
            if (!inventorySlot.activeSelf)
            {
                inventorySlot.SetActive(true);
            }
            if (inventoryItemImage.sprite != heldItem.itemInventoryRender)
            {
                inventoryItemImage.sprite = heldItem.itemInventoryRender;
            }
        }
        else
        {
            if (inventorySlot.activeSelf)
            {
                inventorySlot.SetActive(false);
            }
        }
    }
}