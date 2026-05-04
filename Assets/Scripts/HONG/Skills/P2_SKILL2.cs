using System.Collections;
using UnityEngine;

public class P2_SKILL2 : PlayerAbility
{
    [SerializeField] Animator taseranim;
    [SerializeField] PlayerState PlayerState;
    [SerializeField] Taser taserprefab;
    [SerializeField] Transform bulletspawnPos;
    [SerializeField] float taseranimtime;

    public override void DoUse()
    {
        if (PlayerState.IsFacingRight == true)
        {
            StartCoroutine(ShootTaserRight());
        }
        else if (PlayerState.IsFacingRight == false)
        {
            StartCoroutine(ShootTaserLeft());
        }
        ConsumeUse();
    }

    protected override bool CanPerform()
    {
        return true;
    }

    public override void Update()
    {

        base.Update();
    }

    IEnumerator ShootTaserLeft()
    {
        taseranim.SetBool("Taser", true);
        taseranim.SetBool("IsDoingSomething", true);
        yield return new WaitForSeconds(taseranimtime);
        taseranim.SetBool("Taser", false);
        taseranim.SetBool("IsDoingSomething", false);
        Taser taser = Instantiate(taserprefab, bulletspawnPos.position, Quaternion.identity);
        taser.ShootLeft();
        taser.player = gameObject;
    }

    IEnumerator ShootTaserRight()
    {
        taseranim.SetBool("Taser", true);
        taseranim.SetBool("IsDoingSomething", true);
        yield return new WaitForSeconds(taseranimtime);
        taseranim.SetBool("Taser", false);
        taseranim.SetBool("IsDoingSomething", false);
        Taser taser = Instantiate(taserprefab, bulletspawnPos.position, Quaternion.identity);
        taser.ShootRight();
        taser.player = gameObject;
    }


    public void StopTaser()
    {
        StopAllCoroutines();
    }
}
