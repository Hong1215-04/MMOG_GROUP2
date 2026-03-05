using UnityEngine;

public abstract class UsableBehaviour : MonoBehaviour
{
    [SerializeField] protected int uses = 1;
    protected int currentUses;

    protected virtual void Start()
    {
        currentUses = uses;
    }

    public void ConsumeUse()
    {
        currentUses--;
        DoUse();

        if (currentUses <= 0)
            OnUsesCompleted();
    }

    protected abstract void DoUse();
    protected abstract void OnUsesCompleted();
}