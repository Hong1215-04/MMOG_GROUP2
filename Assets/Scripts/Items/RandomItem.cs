using UnityEngine;

public class RandomItem : Item
{
    [SerializeField] Item[] items;
    Item item;

    public override void DoUse()
    {
    }

    public override void OnPickUp()
    {
        if (items.Length == 0) return;

        Item randomPrefab = items[Random.Range(0, items.Length)];
        item = Instantiate(randomPrefab, transform.position, Quaternion.identity);
        item.player = player;
        player.GetComponent<PlayerItemInteraction>().SetHeldItem(item);
        item.OnPickUp();
        Destroy(gameObject);
    }
}
