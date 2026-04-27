using UnityEngine;

public class PlayerAbilityIconEffects : MonoBehaviour
{
    [SerializeField] IconEffects icons;
    [SerializeField] bool playEffects = true;
    [SerializeField] PlayerAbility ability;

    void Start()
    {
        if (ability == null) return;
        ability.OnUsed += HandleUsed;
        ability.OnCooldownTick += HandleCooldownTick;
        ability.OnCooldownEnd += HandleCooldownEnd;
        ability.OnRefillComplete += HandleRefillComplete;
    }

    void OnDestroy()
    {
        if (ability == null) return;
        ability.OnUsed -= HandleUsed;
        ability.OnCooldownTick -= HandleCooldownTick;
        ability.OnCooldownEnd -= HandleCooldownEnd;
        ability.OnRefillComplete -= HandleRefillComplete;
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