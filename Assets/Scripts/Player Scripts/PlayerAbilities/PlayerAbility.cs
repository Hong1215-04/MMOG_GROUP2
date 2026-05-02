using System;
using UnityEngine;

public abstract class PlayerAbility : UsableBehaviour
{
    [SerializeField] KeyCode abilityKey;
    [SerializeField] float cooldown;
    [SerializeField] float fillDropSpeed = 5f;

    [Header("Auto Refill")]
    [SerializeField] bool autoRefill = false;
    [SerializeField] float refillDelay = 0.5f;

    protected KeyCode AbilityKey => abilityKey;
    public event Action<bool> OnUsed;           // bool = isLastUse
    public event Action OnUseCompleted;
    public event Action<float> OnCooldownTick;   // float = seconds remaining
    public event Action OnCooldownEnd;
    public event Action<float> OnFillChanged;    // float = 0-1 fill amount
    public event Action OnRefillComplete;

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
    float displayFill;

    float FillAmount => currentUses / uses;

    protected override void Start()
    {
        base.Start();
        isInCooldown = false;
        isDroppingFill = false;
        isRefilling = false;
        displayFill = 1f;
    }

    public abstract override void DoUse();
    protected abstract bool CanPerform();

    protected override void OnUsesCompleted()
    {
        StartCooldown();
    }

    public void CompleteUse()
    {
        OnUseCompleted?.Invoke();
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
        if (isRefilling) return;

        refillDuration = (usesSpent / (float)uses) * cooldown;
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

        if (refillDelayTimer < refillDelay)
        {
            refillDelayTimer += Time.deltaTime;
            return;
        }

        if (IsInUse()) return;

        refillTimer += Time.deltaTime;
        float t = Mathf.Clamp01(refillTimer / refillDuration);

        currentUses = Mathf.Lerp(usesAtRefillStart, uses, t);

        if (t >= 1f)
        {
            currentUses = uses;
            isRefilling = false;
            OnRefillComplete?.Invoke();
        }
    }

    // ─────────────────────────────────────────────
    //  Fill tracking
    // ─────────────────────────────────────────────

    void UpdateFill()
    {
        float targetFill;

        if (isInCooldown)
        {
            targetFill = isDroppingFill || isCooldownDelaying
                ? 0f
                : cooldownTimer / cooldown;
        }
        else
        {
            targetFill = FillAmount;
        }

        displayFill = Mathf.Lerp(displayFill, targetFill, Time.deltaTime * fillDropSpeed);
        OnFillChanged?.Invoke(displayFill);

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
            if (isDroppingFill) { /* wait for drop animation */ }
            else if (isCooldownDelaying)
            {
                cooldownDelayTimer += Time.deltaTime;
                if (cooldownDelayTimer >= refillDelay)
                {
                    isCooldownDelaying = false;
                    OnCooldownTick?.Invoke(cooldown);
                }
            }
            else
            {
                float remaining = cooldown - cooldownTimer;
                OnCooldownTick?.Invoke(remaining);

                if (cooldownTimer >= cooldown)
                {
                    isInCooldown = false;
                    cooldownTimer = 0f;
                    currentUses = uses;
                    OnCooldownEnd?.Invoke();
                    OnRefillComplete?.Invoke();
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
                OnUsed?.Invoke(currentUses <= useThreshold);
            }
        }

        HandleAutoRefill();
        UpdateFill();
    }
}