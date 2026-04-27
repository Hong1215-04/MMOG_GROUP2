using UnityEngine;

public class ItemSound : MonoBehaviour
{
    Item item;
    [SerializeField] AudioSource itemUseSound, itemDestroySound;

    private void Start()
    {
        item = GetComponent<Item>();
        item.OnItemDestroyed += PlaySoundOnDestroy;
        item.OnItemUse += PlaySoundOnUse;
    }

    void PlaySoundOnUse()
    {
        if (itemUseSound != null)
        {
            itemUseSound.Play();
        }
    }

    void PlaySoundOnDestroy()
    {
        if (itemUseSound != null)
        {
            {
                itemUseSound.Stop();
            }
        }
        if (itemDestroySound != null)
        {
            itemDestroySound.Play();
        }
    }

}
