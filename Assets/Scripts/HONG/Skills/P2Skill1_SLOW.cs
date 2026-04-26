using UnityEngine;
using System.Collections;

public class P2Skill1_SLOW : PlayerAbility
{
    [SerializeField] Collider2D SlowDectection;
    [SerializeField] float StartupFrame1 = 0.2f;
    [SerializeField] float ActiveFrame1 = 3f;
    [SerializeField] float EndingFrame1 = 0.2f;

    protected override void Start()
    {
        base.Start(); // important!
        SlowDectection.enabled = false;
    }

    public override void DoUse()
    {
        StartCoroutine(DetectionOut());
    }

    protected override bool CanPerform()
    {
        return true;
    }

    IEnumerator DetectionOut()
    {
        yield return new WaitForSeconds(StartupFrame1);
        SlowDectection.enabled = true;
        yield return new WaitForSeconds(ActiveFrame1);
        SlowDectection.enabled = false;
        yield return new WaitForSeconds(EndingFrame1);
    }

    public void skillhitted()
    {
        SlowDectection.enabled = false;
    }
}
