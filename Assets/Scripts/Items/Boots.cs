using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Boots : Item
{
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] float speedMultiplier = 1.5f;
    [SerializeField] float duration = 5f;

    PlayerMovement movement;

    public override void OnPickUp()
    {
        rb.simulated = false;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
        SetParentGO(spriteRenderers[0].transform, player.GetComponent<PlayerState>().BootHolder1);
        SetParentGO(spriteRenderers[1].transform, player.GetComponent<PlayerState>().BootHolder2);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
            if(spriteRenderer.transform.localScale.x > 0)
            {
                float xScale = spriteRenderer.transform.localScale.x * -1;
                Vector3 newScale = new Vector3(xScale, spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
                spriteRenderer.transform.localScale = newScale;
            }
        }
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        movement = player.GetComponent<PlayerMovement>();
    }

    void SetParentGO(Transform target, Transform parentGo)
    {
        target.SetParent(parentGo);
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;
    }

    public override void DoUse()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = true;
        }
        if (movement == null) return;
        movement.AddSpeedMultiplier(speedMultiplier);
        StartCoroutine(SpeedRoutine());
    }

    protected override void OnUsesCompleted()
    {
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Destroy(spriteRenderer.gameObject);
        }
        Destroy(gameObject);
    }
    IEnumerator SpeedRoutine()
    {
        yield return new WaitForSeconds(duration);
        movement.RemoveSpeedMultiplier(speedMultiplier);
        ConsumeUse();
    }
}