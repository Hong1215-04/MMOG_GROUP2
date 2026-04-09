using UnityEngine;
using System.Collections;

public class P2Skill1_SLOW : PlayerAbility
{
    [SerializeField] GameObject SlowDectection;
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
        SlowDectection.SetActive(true);
        yield return new WaitForSeconds(2f);
        SlowDectection.SetActive(false);
    }
}
