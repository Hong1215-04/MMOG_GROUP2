using System.Collections;
using UnityEngine;

public static class UIAnimator
{
    public static IEnumerator SlideToPosition(Transform target, Vector3 from, Vector3 to, float speed)
    {
        target.position = from;
        float distance = Vector3.Distance(from, to);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (target == null) yield break;
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            target.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        target.position = to;
    }
}