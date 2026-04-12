using UnityEngine;

public class PlayerHitSound : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] AudioSource hitSource;

    private void OnEnable()
    {
        health.OnDamageTaken += PlayHitSound;
    }

    public void PlayHitSound()
    {
        hitSource.Play();
    }


    private void OnDisable()
    {
        health.OnDamageTaken -= PlayHitSound;
    }

}
