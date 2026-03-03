using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] Image uiElement;
    [SerializeField] KeyCode abilityKey;
    [SerializeField] float cooldown;
    [SerializeField] float fillDropSpeed = 5f;
    float cooldownTimer;
    bool isInCooldown;
    bool isDroppingFill;

    private void Start()
    {
        isInCooldown = false;
        isDroppingFill = false;
        if (uiElement != null)
        {
            uiElement.fillAmount = 1f;
            uiElement.type = Image.Type.Filled;
        }
    }

    protected virtual void DoAbility()
    {
        Debug.Log("DO ABILITY!!!");
        isInCooldown = true;
        isDroppingFill = true;
        cooldownTimer = 0f;
    }

    void UpdateFill()
    {
        if (uiElement == null) return;

        if (isDroppingFill)
        {
            uiElement.fillAmount = Mathf.Lerp(uiElement.fillAmount, 0f, Time.deltaTime * fillDropSpeed);

            if (uiElement.fillAmount <= 0.15f)
            {
                uiElement.fillAmount = 0f;
                isDroppingFill = false;
            }
        }
        else if (isInCooldown)
        {
            uiElement.fillAmount = Mathf.Lerp(uiElement.fillAmount, cooldownTimer / cooldown, Time.deltaTime * fillDropSpeed);
        }
        else
        {
            uiElement.fillAmount = Mathf.Lerp(uiElement.fillAmount, 1f, Time.deltaTime * fillDropSpeed);
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
                }
                else
                {
                    cooldownTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(abilityKey))
            {
                DoAbility();
            }
        }

        UpdateFill();
    }
}