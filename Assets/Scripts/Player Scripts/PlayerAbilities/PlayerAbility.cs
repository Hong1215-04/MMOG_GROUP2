using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerAbility : UsableBehaviour
{
    [SerializeField] Image uiElement;
    [SerializeField] KeyCode abilityKey;
    [SerializeField] float cooldown;
    [SerializeField] float fillDropSpeed = 5f;

    [Header("Auto Refill")]
    [Tooltip("Automatically refill uses when below max. Refill duration scales with how much was used.")]
    [SerializeField] bool autoRefill = false;

    [Tooltip("Seconds to wait after releasing before refill begins")]
    [SerializeField] float refillDelay = 2f;

    protected KeyCode AbilityKey => abilityKey;

    float cooldownTimer;
    float refillTimer;
    float refillDelayTimer;
    float refillDuration;   // calculated from how much was used
    float usesAtRefillStart;
    bool isInCooldown;
    bool isDroppingFill;
    bool isRefilling;

    float FillAmount => currentUses / uses;

    protected override void Start()
    {
        base.Start();
        isInCooldown = false;
        isDroppingFill = false;
        isRefilling = false;
        if (uiElement != null)
        {
            uiElement.fillAmount = 1f;
            uiElement.type = Image.Type.Filled;
        }
    }

    protected abstract override void DoUse();
    protected abstract bool CanPerform();

    protected override void OnUsesCompleted()
    {
        StartCooldown();
    }

    protected void StartCooldown()
    {
        isInCooldown = true;
        cooldownTimer = 0f;
        isDroppingFill = true;
        isRefilling = false;
    }

    /// <summary>
    /// Call this when the ability finishes using so the refill can start.
    /// usesSpent = how many uses were consumed this activation.
    /// Refill duration = (usesSpent / maxUses) * cooldown
    /// e.g. used 1.5 of 3, cooldown = 6 → refill takes 3s
    /// </summary>
    protected void StartRefill(float usesSpent)
    {
        if (!autoRefill || isInCooldown) return;

        refillDuration = (usesSpent / uses) * cooldown;
        refillTimer = 0f;
        refillDelayTimer = 0f;
        usesAtRefillStart = currentUses;
        isRefilling = true;
    }

    protected virtual bool IsInUse() => false;

    // ─────────────────────────────────────────────
    //  Auto Refill
    // ─────────────────────────────────────────────

    void HandleAutoRefill()
    {
        if (!isRefilling) return;
        if (isInCooldown) { isRefilling = false; return; }
        if (IsInUse()) return;

        // Wait for the delay before starting to refill
        if (refillDelayTimer < refillDelay)
        {
            refillDelayTimer += Time.deltaTime;
            return;
        }

        refillTimer += Time.deltaTime;
        float t = Mathf.Clamp01(refillTimer / refillDuration);

        // Smoothly restore uses from where they were when refill started back to max
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

        if (isInCooldown)
        {
            if (isDroppingFill)
            {
                // Snap to empty quickly then hand off to cooldown fill
                uiElement.fillAmount = Mathf.Lerp(uiElement.fillAmount, 0f, Time.deltaTime * fillDropSpeed);
                if (uiElement.fillAmount <= 0.01f)
                {
                    uiElement.fillAmount = 0f;
                    isDroppingFill = false;
                }
            }
            else
            {
                // Fill bar tracks cooldown progress linearly
                uiElement.fillAmount = cooldownTimer / cooldown;
            }
        }
        else
        {
            // Always track FillAmount directly — covers draining and refilling smoothly
            uiElement.fillAmount = FillAmount;
        }
    }

    // ─────────────────────────────────────────────
    //  Update
    // ─────────────────────────────────────────────

    public virtual void Update()
    {
        if (isInCooldown)
        {
            if (!isDroppingFill)
            {
                if (cooldownTimer >= cooldown)
                {
                    isInCooldown = false;
                    cooldownTimer = 0f;
                    currentUses = uses;
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
                DoUse();
        }

        HandleAutoRefill();
        UpdateFill();
    }
}