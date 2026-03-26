using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerAbility : UsableBehaviour
{
    [SerializeField] Image uiElement;
    IconEffects icons;
    [SerializeField] KeyCode abilityKey;
    [SerializeField] float cooldown;
    [SerializeField] float fillDropSpeed = 5f;
    [SerializeField] bool playEffects = true;
    [Header("Auto Refill")]
    [Tooltip("Automatically refill uses when below max. Refill duration scales with how much was used.")]
    [SerializeField] bool autoRefill = false;

    [Tooltip("Seconds to wait after releasing before refill begins")]
    [SerializeField] float refillDelay = 0.5f;

    

    protected KeyCode AbilityKey => abilityKey;

    float cooldownTimer;
    float cooldownDelayTimer;
    bool isCooldownDelaying;
    float refillTimer;
    float refillDelayTimer;
    float refillDuration;
    float usesAtRefillStart;
    bool isInCooldown;
    bool isDroppingFill;
    bool isRefilling;
    float displayFill;   // smoothed value driving the UI — lerps toward the real FillAmount

    float FillAmount => currentUses / uses;

    protected override void Start()
    {
        base.Start();
        isInCooldown = false;
        isDroppingFill = false;
        isRefilling = false;
        displayFill = 1f;
        if (uiElement != null)
        {
            if(uiElement.GetComponentInParent<IconEffects>() != null)
            {
                icons = uiElement.GetComponentInParent<IconEffects>();
            }
            uiElement.fillAmount = 1f;
            uiElement.type = Image.Type.Filled;
        }
    }

    public abstract override void DoUse();
    protected abstract bool CanPerform();

    protected override void OnUsesCompleted()
    {
        StartCooldown();
    }

    protected void StartCooldown()
    {
        isInCooldown = true;
        isCooldownDelaying = true;
        cooldownDelayTimer = 0f;
        cooldownTimer = 0f;
        isDroppingFill = true;
        isRefilling = false;
    }

    protected void StartRefill(float usesSpent)
    {
        if (!autoRefill || isInCooldown) return;
        if (isRefilling) return;   // already refilling — don't reset the timer

        refillDuration = (usesSpent / (float)uses) * cooldown;
        refillTimer = 0f;
        refillDelayTimer = 0f;
        usesAtRefillStart = currentUses;
        isRefilling = true;

        Debug.Log($"StartRefill — usesSpent:{usesSpent} uses:{uses} cooldown:{cooldown} refillDuration:{refillDuration} usesAtStart:{usesAtRefillStart}");
    }

    protected virtual bool IsInUse() => false;

    // ─────────────────────────────────────────────
    //  Auto Refill
    // ─────────────────────────────────────────────

    void HandleAutoRefill()
    {
        if (!isRefilling) return;
        if (isInCooldown) { isRefilling = false; return; }

        // Delay timer always ticks so it doesn't stack on top of IsInUse duration
        if (refillDelayTimer < refillDelay)
        {
            refillDelayTimer += Time.deltaTime;
            return;
        }

        // Only block the actual refill while the ability is still running
        if (IsInUse()) return;

        refillTimer += Time.deltaTime;
        float t = Mathf.Clamp01(refillTimer / refillDuration);

        currentUses = Mathf.Lerp(usesAtRefillStart, uses, t);

        if (t >= 1f)
        {
            currentUses = uses;
            isRefilling = false;
        }
    }

    // ─────────────────────────────────────────────
    //  UI Fill
    // ─────────────────────────────────────────────

    void UpdateFill()
    {
        if (uiElement == null) return;

        float targetFill;

        if (isInCooldown)
        {
            if (isDroppingFill)
            {
                targetFill = 0f;
            }
            else if (isCooldownDelaying)
            {
                targetFill = 0f;
            }
            else
            {
                // Direct assign for cooldown — it should feel like a clean linear fill
                targetFill = cooldownTimer / cooldown;
            }
        }
        else
        {
            // Tracks currentUses — smooth for instant drops and gradual drains/refills
            targetFill = FillAmount;
        }

        displayFill = Mathf.Lerp(displayFill, targetFill, Time.deltaTime * fillDropSpeed);
        uiElement.fillAmount = displayFill;

        // Once display reaches near zero during drop, snap and hand off
        if (isDroppingFill && displayFill <= 0.01f)
        {
            displayFill = 0f;
            isDroppingFill = false;
        }
    }

    // ─────────────────────────────────────────────
    //  Update
    // ─────────────────────────────────────────────

    public virtual void Update()
    {
        if (isInCooldown)
        {
            if (isDroppingFill) { /* wait for drop animation to finish */ }
            else if (isCooldownDelaying)
            {
                cooldownDelayTimer += Time.deltaTime;
                if (cooldownDelayTimer >= refillDelay)
                {
                    isCooldownDelaying = false;
                    icons?.UpdateCooldownText(cooldown);
                }
            }
            else
            {
                float remaining = cooldown - cooldownTimer;
                icons?.UpdateCooldownText(remaining);

                if (cooldownTimer >= cooldown)
                {
                    isInCooldown = false;
                    cooldownTimer = 0f;
                    currentUses = uses;
                    icons?.HideCooldownText();
                    icons?.OnAbilityRefilled();
                }
                else
                {
                    cooldownTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(abilityKey) && CanPerform() && currentUses > 0)
            {
                DoUse();
                if(icons != null && playEffects)
                {
                    icons?.OnAbilityUsed(currentUses <= useThreshold);
                }
            }

        }

        HandleAutoRefill();
        UpdateFill();
    }
}