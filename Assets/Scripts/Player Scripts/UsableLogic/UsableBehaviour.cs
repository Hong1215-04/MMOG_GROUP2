using UnityEngine;

public abstract class UsableBehaviour : MonoBehaviour
{
    [SerializeField] protected float uses = 1;
    [SerializeField] float useThreshold = 0.001f;   // add this
    protected float currentUses;

    protected virtual void Start()
    {
        currentUses = uses;
    }

    public void ConsumeUse(float used = 1f)
    {
        currentUses -= used;
        if (currentUses < useThreshold)
        {
            currentUses = 0;
            OnUsesCompleted();
        }
    }

    public abstract void DoUse();
    protected abstract void OnUsesCompleted();
}