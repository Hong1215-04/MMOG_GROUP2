using UnityEngine;

public class ItemPickupSound : MonoBehaviour
{
    PlayerItemInteraction playerItemInteraction;
    [SerializeField] AudioSource onPickupSource;

    private void Start()
    {
        playerItemInteraction = GetComponentInParent<PlayerItemInteraction>();
        playerItemInteraction.OnItemPickup += PlayPickUp;
    }

    void PlayPickUp()
    {
        onPickupSource.Play();
    }

    private void OnDestroy()
    {
        playerItemInteraction.OnItemPickup -= PlayPickUp;
    }

}
