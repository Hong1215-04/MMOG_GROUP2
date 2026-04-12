using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityFill : MonoBehaviour
{
    [SerializeField] Image uiElement;
    [SerializeField] PlayerAbility ability;

    void Start()
    {
        if (ability == null) return;
        ability.OnFillChanged += HandleFillChanged;

        if (uiElement != null)
        {
            uiElement.fillAmount = 1f;
            uiElement.type = Image.Type.Filled;
        }
    }

    void OnDestroy()
    {
        if (ability != null)
            ability.OnFillChanged -= HandleFillChanged;
    }

    void HandleFillChanged(float fill)
    {
        if (uiElement != null)
            uiElement.fillAmount = fill;
    }
}