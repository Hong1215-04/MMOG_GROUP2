using System.Collections;
using UnityEngine;

public class QuestionMarkPositionSwitcher : Item
{
    [SerializeField] SpriteRenderer[] spriteRenderers;

    public override void DoUse()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = true;
        }
        player.GetComponent<PlayerState>().HeadAnim.SetTrigger("Move");
        PlayerMovement[] allPlayers = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        PlayerMovement otherPlayer = null;

        foreach (PlayerMovement p in allPlayers)
        {
            if (p.gameObject != player.gameObject)
            {
                otherPlayer = p;
                break;
            }
        }

        if (otherPlayer == null) return;

        Vector3 thisPlayerPos = player.transform.position;
        player.transform.position = otherPlayer.transform.position;
        otherPlayer.transform.position = thisPlayerPos;

        ConsumeUse();
    }

    protected override void OnUsesCompleted()
    {
        StartCoroutine(Finish());
    }

    IEnumerator Finish()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        transform.SetParent(player.GetComponent<PlayerState>().HeadSlot);
        transform.localPosition = Vector3.zero;
        rb.simulated = false;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
    }
}