using UnityEngine;

public abstract class UsableBehaviour : MonoBehaviour
{
    [SerializeField] protected int uses = 1;

    // float so abilities can drain uses continuously over time (e.g. hold-to-dash)
    protected float currentUses;

    protected virtual void Start()
    {
        currentUses = uses;
    }

    public void ConsumeUse(float used = 1f)
    {
        currentUses -= used;
        if (currentUses <= 0)
        {
            currentUses = 0;
            OnUsesCompleted();
        }
    }

    public abstract void DoUse();
    protected abstract void OnUsesCompleted();
}