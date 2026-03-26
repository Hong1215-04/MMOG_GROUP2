using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// IconEffects — Visual feedback for ability icons.
///
/// Exposes methods for PlayerAbility to call at the right moments:
///   OnAbilityUsed()       — call once when the ability activates
///   OnAbilityRefilled()   — call when uses are fully restored
///   UpdateCooldownText()  — call every frame during cooldown
///   HideCooldownText()    — call when cooldown ends
/// </summary>
public class IconEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform iconRect;
    [SerializeField] RectTransform maskRect;
    public Image skillImage;
    [SerializeField] Image sparkleImage;
    public TextMeshProUGUI cooldownText;

    [Header("Icon Pulse (on use)")]
    [SerializeField] Vector2 iconPunchScale = new Vector2(1.3f, 1.3f);
    [SerializeField] float iconPunchTime = 0.12f;
    [SerializeField] float iconReturnTime = 0.2f;

    [Header("Mask Expand (on use)")]
    [SerializeField] Vector2 maskStartSize = new Vector2(25f, 25f);
    [SerializeField] Vector2 maskExpandSize = new Vector2(200f, 200f);
    [SerializeField] float maskExpandTime = 0.25f;
    [SerializeField] float maskReturnTime = 0.3f;

    [Header("Sparkle (on use)")]
    [SerializeField] float sparklePeakAlpha = 1f;
    [SerializeField] float sparkleFadeTime = 0.35f;

    [Header("Refill Flash (on full refill)")]
    [SerializeField] float refillAlphaFrom = 0.4f;
    [SerializeField] float refillAlphaTo = 1f;
    [SerializeField] float refillScalePeak = 1.1f;
    [SerializeField] float refillFlashTime = 0.12f;
    [SerializeField] float refillReturnTime = 0.2f;
    [Tooltip("Scale the icon sits at while on cooldown (e.g. 0.8)")]
    [SerializeField] float cooldownScale = 0.8f;

    Vector2 iconBaseScale;
    Coroutine iconRoutine;
    Coroutine maskRoutine;
    Coroutine sparkleRoutine;

    void Awake()
    {
        iconBaseScale = iconRect != null ? iconRect.localScale : Vector2.one;

        if (sparkleImage != null)
            SetAlpha(sparkleImage, 0f);

        if (maskRect != null)
            maskRect.sizeDelta = maskStartSize;

        if (cooldownText != null)
            cooldownText.gameObject.SetActive(false);

        // Start at full alpha — ability is ready
        if (skillImage != null)
            SetAlpha(skillImage, refillAlphaTo);
    }

    // ─────────────────────────────────────────────
    //  Public triggers
    // ─────────────────────────────────────────────

    /// <summary>Call once when the ability is activated.</summary>
    public void OnAbilityUsed(bool isLastUse)
    {
        if (iconRect != null) RestartCoroutine(ref iconRoutine, PunchIcon(isLastUse));
        if (maskRect != null) RestartCoroutine(ref maskRoutine, ExpandMask());
        if (sparkleImage != null) RestartCoroutine(ref sparkleRoutine, FlashSparkle());
    }

    /// <summary>Call once when uses are fully restored (cooldown end or full auto-refill).</summary>
    public void OnAbilityRefilled()
    {
        if (skillImage != null) RestartCoroutine(ref iconRoutine, RefillFlash());
    }

    /// <summary>Call every frame during cooldown with seconds remaining.</summary>
    public void UpdateCooldownText(float secondsRemaining)
    {
        if (cooldownText == null) return;
        cooldownText.gameObject.SetActive(true);
        cooldownText.text = secondsRemaining > 0f
            ? secondsRemaining.ToString("F1")
            : string.Empty;
    }

    /// <summary>Call when cooldown ends to hide the text.</summary>
    public void HideCooldownText()
    {
        if (cooldownText == null) return;
        cooldownText.gameObject.SetActive(false);
    }

    // ─────────────────────────────────────────────
    //  Icon — punch scale on use, bounce back to normal
    // ─────────────────────────────────────────────
    IEnumerator PunchIcon(bool settleAtCooldown)
    {
        Vector2 settleScale = settleAtCooldown
            ? iconBaseScale * cooldownScale  // shrink to 0.8 — no uses left
            : iconBaseScale;                 // stay at 1.0 — uses remain

        float settleAlpha = settleAtCooldown ? refillAlphaFrom : refillAlphaTo;

        // Punch up
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / iconPunchTime;
            if (iconRect != null) iconRect.localScale = Vector2.Lerp(iconBaseScale, iconPunchScale, Easing.EaseOutBack(Mathf.Clamp01(t)));
            yield return null;
        }

        // Settle to either cooldown state or ready state
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / iconReturnTime;
            float e = Mathf.Clamp01(t);
            if (iconRect != null) iconRect.localScale = Vector2.Lerp(iconPunchScale, settleScale, Easing.EaseInOutQuad(e));
            if (skillImage != null) SetAlpha(skillImage, Mathf.Lerp(refillAlphaTo, settleAlpha, Easing.EaseInOutQuad(e)));
            yield return null;
        }

        if (iconRect != null) iconRect.localScale = settleScale;
        if (skillImage != null) SetAlpha(skillImage, settleAlpha);
    }
    // ─────────────────────────────────────────────
    //  Refill effects — all run simultaneously
    // ─────────────────────────────────────────────

    IEnumerator RefillFlash()
    {
        Vector2 cooldownScaleVec = iconBaseScale * cooldownScale;
        Vector2 peakScale = iconBaseScale * refillScalePeak;

        // Flash up from cooldown state (0.8 scale, 0.4 alpha) to peak
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / refillFlashTime;
            float e = Easing.EaseOutQuad(Mathf.Clamp01(t));
            if (iconRect != null) iconRect.localScale = Vector2.Lerp(cooldownScaleVec, peakScale, e);
            if (skillImage != null) SetAlpha(skillImage, Mathf.Lerp(refillAlphaFrom, refillAlphaTo, e));
            yield return null;
        }

        // Settle to base scale — alpha stays at 1.0 permanently (ready state)
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / refillReturnTime;
            float e = Easing.EaseInOutQuad(Mathf.Clamp01(t));
            if (iconRect != null) iconRect.localScale = Vector2.Lerp(peakScale, iconBaseScale, e);
            yield return null;
        }

        // Lock in ready state
        if (iconRect != null) iconRect.localScale = iconBaseScale;
        if (skillImage != null) SetAlpha(skillImage, refillAlphaTo);
    }

    // 2. Ring ping — expands outward fast then snaps back instantly
    IEnumerator RefillPing()
    {
        maskRect.sizeDelta = maskStartSize;
        yield return TweenSize(maskRect, maskStartSize, maskExpandSize, 0.3f, Easing.EaseOutQuart);
        maskRect.sizeDelta = maskStartSize;
    }

    // 3. Sparkle burst — faster and snappier than on-use
    IEnumerator RefillSparkle()
    {
        yield return TweenAlpha(sparkleImage, 0f, sparklePeakAlpha, 0.08f, Easing.EaseOutQuad);
        yield return TweenAlpha(sparkleImage, sparklePeakAlpha, 0f, 0.25f, Easing.EaseInQuad);
    }

    // 4. Icon image flashes white then returns — "ready" signal
    IEnumerator RefillBrighten()
    {
        Color original = skillImage.color;
        yield return TweenColor(skillImage, original, Color.white, 0.07f, Easing.EaseOutQuad);
        yield return TweenColor(skillImage, Color.white, original, 0.2f, Easing.EaseInOutQuad);
    }

    // ─────────────────────────────────────────────
    //  Mask — expand outward then collapse back
    // ─────────────────────────────────────────────

    IEnumerator ExpandMask()
    {
        yield return TweenSize(maskRect, maskStartSize, maskExpandSize, maskExpandTime, Easing.EaseOutQuart);
        yield return TweenSize(maskRect, maskExpandSize, maskStartSize, maskReturnTime, Easing.EaseInQuad);
    }

    // ─────────────────────────────────────────────
    //  Sparkle — fade in with mask, then fade out
    // ─────────────────────────────────────────────

    IEnumerator FlashSparkle()
    {
        yield return TweenAlpha(sparkleImage, 0f, sparklePeakAlpha, maskExpandTime, Easing.EaseOutQuad);
        yield return TweenAlpha(sparkleImage, sparklePeakAlpha, 0f, sparkleFadeTime, Easing.EaseInQuad);
    }

    // ─────────────────────────────────────────────
    //  Tween helpers
    // ─────────────────────────────────────────────

    IEnumerator TweenScale(RectTransform target, Vector2 from, Vector2 to, float duration, System.Func<float, float> ease)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            target.localScale = Vector2.Lerp(from, to, ease(Mathf.Clamp01(t)));
            yield return null;
        }
        target.localScale = to;
    }

    IEnumerator TweenSize(RectTransform target, Vector2 from, Vector2 to, float duration, System.Func<float, float> ease)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            target.sizeDelta = Vector2.Lerp(from, to, ease(Mathf.Clamp01(t)));
            yield return null;
        }
        target.sizeDelta = to;
    }

    IEnumerator TweenAlpha(Image target, float from, float to, float duration, System.Func<float, float> ease)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            SetAlpha(target, Mathf.Lerp(from, to, ease(Mathf.Clamp01(t))));
            yield return null;
        }
        SetAlpha(target, to);
    }

    IEnumerator TweenColor(Image target, Color from, Color to, float duration, System.Func<float, float> ease)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            target.color = Color.Lerp(from, to, ease(Mathf.Clamp01(t)));
            yield return null;
        }
        target.color = to;
    }

    void RestartCoroutine(ref Coroutine slot, IEnumerator routine)
    {
        if (slot != null) StopCoroutine(slot);
        slot = StartCoroutine(routine);
    }

    static void SetAlpha(Image img, float a)
    {
        Color c = img.color;
        c.a = a;
        img.color = c;
    }

    // ─────────────────────────────────────────────
    //  Easing library — no dependencies needed
    // ─────────────────────────────────────────────

    static class Easing
    {
        public static float EaseOutBack(float t)
        {
            float c1 = 1.70158f, c3 = c1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }
        public static float EaseInOutQuad(float t) =>
            t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        public static float EaseOutQuart(float t) =>
            1f - Mathf.Pow(1f - t, 4f);
        public static float EaseOutElastic(float t)
        {
            if (t == 0f || t == 1f) return t;
            float c4 = (2f * Mathf.PI) / 3f;
            return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
        }
        public static float EaseInQuad(float t) => t * t;
        public static float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);
    }
}