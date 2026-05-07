using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityIconEffects : MonoBehaviour
{
    [SerializeField] IconEffects icons;
    [SerializeField] bool playEffects = true;
    [SerializeField] GameObject banImage;
    public PlayerAbility ability;

    void Start()
    {
        if (ability == null) return;
        ability.OnUsed += HandleUsed;
        ability.OnCooldownTick += HandleCooldownTick;
        ability.OnCooldownEnd += HandleCooldownEnd;
        ability.OnRefillComplete += HandleRefillComplete;
        ability.OnAbilityDisabled += ShowBannedImage;
        ability.OnAbilityEnabled += HideBannedImage;
    }

    void ShowBannedImage()
    {
        if(banImage != null)
        {
            banImage.SetActive(true);
        }
    }
    void HideBannedImage()
    {
        if (banImage != null)
        {
            banImage.SetActive(false);
        }
    }
    void OnDestroy()
    {
        if (ability == null) return;
        ability.OnUsed -= HandleUsed;
        ability.OnCooldownTick -= HandleCooldownTick;
        ability.OnCooldownEnd -= HandleCooldownEnd;
        ability.OnRefillComplete -= HandleRefillComplete;
        ability.OnAbilityDisabled -= ShowBannedImage;
        ability.OnAbilityEnabled -= HideBannedImage;
    }

    void HandleUsed(bool isLastUse)
    {
        if (playEffects) icons?.OnAbilityUsed(isLastUse);
    }

    void HandleCooldownTick(float remaining)
    {
        icons?.UpdateCooldownText(remaining);
    }

    void HandleCooldownEnd()
    {
        icons?.HideCooldownText();
    }

    void HandleRefillComplete()
    {
        if (playEffects) icons?.OnAbilityRefilled();
    }
}