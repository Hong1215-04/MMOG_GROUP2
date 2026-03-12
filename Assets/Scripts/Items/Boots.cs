using System.Collections;
using UnityEngine;

public class Boots : Item
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float speedMultiplier = 1.5f;
    [SerializeField] float duration = 5f;

    PlayerMovement movement;

    public override void OnPickUp()
    {
        rb.simulated = false;
        spriteRenderer.enabled = false;
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        movement = player.GetComponent<PlayerMovement>();
    }

    public override void DoUse()
    {
        spriteRenderer.enabled = true;
        if (movement == null) return;
        movement.AddSpeedMultiplier(speedMultiplier);
        StartCoroutine(SpeedRoutine());
    }

    IEnumerator SpeedRoutine()
    {
        yield return new WaitForSeconds(duration);
        movement.RemoveSpeedMultiplier(speedMultiplier);
        ConsumeUse();
    }
}