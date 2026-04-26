using UnityEngine;
using System.Collections;

public class P2Skill1_SLOW : PlayerAbility
{
    [SerializeField] GameObject SlowDectection;
    [SerializeField] float StartupFrame1 = 0.2f;
    [SerializeField] float ActiveFrame1 = 0.2f;
    [SerializeField] float EndingFrame1 = 0.2f;

    protected override void Start()
    {
        base.Start(); // important!
        SlowDectection.SetActive(false);
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
        SlowDectection.SetActive(true);
        yield return new WaitForSeconds(ActiveFrame1);
        SlowDectection.SetActive(false);
        yield return new WaitForSeconds(EndingFrame1);
    }
}
