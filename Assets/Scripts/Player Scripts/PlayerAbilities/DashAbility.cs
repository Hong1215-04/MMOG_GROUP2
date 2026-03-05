using UnityEngine;
public class DashAbility : PlayerAbility
{
    protected override bool CanPerform() => true;

    protected override void DoUse()
    {
        Debug.Log("Dash!");
    }
}