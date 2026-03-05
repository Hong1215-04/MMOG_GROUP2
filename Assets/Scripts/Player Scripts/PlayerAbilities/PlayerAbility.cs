using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerAbility : UsableBehaviour
{
    [SerializeField] Image uiElement;
    [SerializeField] KeyCode abilityKey;
    [SerializeField] float cooldown;
    [SerializeField] float fillDropSpeed = 5f;

    float cooldownTimer;
    bool isInCooldown;
    bool isDroppingFill;

    float FillAmount => (float)currentUses / uses;

    protected override void Start()
    {
        base.Start();
        isInCooldown = false;
        isDroppingFill = false;

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
    }

    void UpdateFill()
    {
        if (uiElement == null) return;

        if (isDroppingFill)
        {
            uiElement.fillAmount = Mathf.Lerp(uiElement.fillAmount, FillAmount, Time.deltaTime * fillDropSpeed);

            if (Mathf.Abs(uiElement.fillAmount - FillAmount) <= 0.15f)
            {
                uiElement.fillAmount = FillAmount;
                isDroppingFill = false;
            }
        }
        else if (isInCooldown)
        {
            uiElement.fillAmount = Mathf.Lerp(uiElement.fillAmount, cooldownTimer / cooldown, Time.deltaTime * fillDropSpeed);
        }
        else
        {
            uiElement.fillAmount = Mathf.Lerp(uiElement.fillAmount, FillAmount, Time.deltaTime * fillDropSpeed);
        }
    }

    protected virtual void Update()
    {
        if (isInCooldown)
        {
            if (!isDroppingFill)
            {
                if (cooldownTimer >= cooldown)
                {
                    isInCooldown = false;
                    cooldownTimer = cooldown;
                    currentUses = uses; // Restore uses after cooldown
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
                ConsumeUse();
            }
        }

        UpdateFill();
    }
}