using UnityEngine;

public class FlashGrenade : Item
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float flashDuration = 1.5f;
    [SerializeField] GameObject explosion;

    [SerializeField] float throwForce = 8f;
    [SerializeField] float torqueForce = 5f;

    bool thrown;
    float thrownTime;

    public override void DoUse()
    {
        transform.SetParent(null);

        spriteRenderer.enabled = true;

        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Vector2 direction = player.transform.right;
        if (!player.GetComponent<PlayerState>().IsFacingRight)
        {
            direction *= -1;
        }
        rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-torqueForce, torqueForce), ForceMode2D.Impulse);

        thrown = true;
        thrownTime = Time.time;
    }

    public override void OnPickUp()
    {
        rb.simulated = false;

        spriteRenderer.enabled = false;

        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;

        thrown = false;
    }

    private void Update()
    {
        if (!thrown) return;

        if (Time.time - thrownTime >= flashDuration)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                ConsumeUse();
            }
        }
    }
}
