using UnityEditor;
using UnityEngine;

public class GasBomb : Item
{
    [SerializeField] SpriteRenderer BombSprite;
    [SerializeField] GameObject GasArea;
    [SerializeField] float explodetime = 1.5f;

    [SerializeField] float throwForce = 8f;
    [SerializeField] float torqueForce = 5f;

    bool thrown;
    float thrownTime;

    protected override void Start()
    {
        //GasArea.SetActive(false);
        base.Start();
    }
    public override void DoUse()
    {
        transform.SetParent(null);
        BombSprite.enabled = true;

        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Vector2 direction = player.transform.right;
        rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-torqueForce, torqueForce), ForceMode2D.Impulse);

        thrown = true;
        thrownTime = Time.time;
    }

    public override void OnPickUp()
    {
        rb.simulated = false;
        BombSprite.enabled = false;

        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (!thrown) return;

        if (Time.time - thrownTime >= explodetime)
        {
            if (GasArea != null)
            {
                Instantiate(GasArea, transform.position, Quaternion.identity);
                ConsumeUse();
            }
        }
    }

}
