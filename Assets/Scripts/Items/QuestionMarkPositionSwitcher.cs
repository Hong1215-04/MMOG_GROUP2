using UnityEngine;

public class QuestionMarkPositionSwitcher : Item
{
    [SerializeField] SpriteRenderer[] spriteRenderers;

    public override void DoUse()
    {
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

    public override void OnPickUp()
    {
        rb.simulated = false;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }
}