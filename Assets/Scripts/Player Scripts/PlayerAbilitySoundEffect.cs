using UnityEngine;

public class PlayerAbilitySoundEffect : MonoBehaviour
{
    [SerializeField] PlayerAbility ability;
    [SerializeField] AudioSource abilitySoundSource, endSoundSource;

    private void OnEnable()
    {
        ability.OnUsed += PlaySound;
        ability.OnUseCompleted += PlayEndSound;
    }

    void PlaySound(bool placeHolder)
    {
        if (abilitySoundSource != null)
        {
            abilitySoundSource.Play();
        }
    }

    void PlayEndSound()
    {
        abilitySoundSource.Stop();
        if(endSoundSource != null)
        {
            endSoundSource.Play();
        }
    }

    private void OnDestroy()
    {
        ability.OnUsed -= PlaySound;
        ability.OnUseCompleted -= PlayEndSound;
    }
}
