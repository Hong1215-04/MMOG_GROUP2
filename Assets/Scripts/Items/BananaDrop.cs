using UnityEngine;

public class BananaDrop : MonoBehaviour
{
    [SerializeField] StunOnEnter bananaStun;
    [SerializeField] Collider2D triggerCol, phCol;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float launchForce = 6f;
    [SerializeField] float sideForce = 2f;
    [SerializeField] float destroyDelay = 1.5f;

    private void OnEnable()
    {
        bananaStun.OnEnterStun += BananaDropEffect;
    }

    void BananaDropEffect()
    {
        triggerCol.enabled = false;
        phCol.enabled = false;

        bananaStun.stunPlayer = false;

        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;

        float randomX = Random.Range(-sideForce, sideForce);
        Vector2 force = new Vector2(randomX, launchForce);

        rb.AddForce(force, ForceMode2D.Impulse);

        Destroy(gameObject, destroyDelay);
    }

    private void OnDestroy()
    {
        bananaStun.OnEnterStun -= BananaDropEffect;
    }
}
