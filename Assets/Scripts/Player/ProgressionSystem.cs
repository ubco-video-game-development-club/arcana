using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressionSystem : MonoBehaviour
{
    public bool HasBuffRune { get; private set; }
    public bool HasAbilityRune { get; private set; }
    public bool HasBlessingRune { get; private set; }
    public bool HasKeyRune { get; private set; }

    public UnityEvent<Runestone> OnRunestoneAcquired { get; private set; }

    private List<Runestone> runestones = new List<Runestone>();

    void Awake()
    {
        OnRunestoneAcquired = new UnityEvent<Runestone>();
    }

    public void AcquireRunestone(Runestone runestone)
    {
        runestones.Add(runestone);
        switch (runestone.RunestoneType)
        {
            case RunestoneType.Buff:
                HasBuffRune = true;
                break;
            case RunestoneType.Ability:
                HasAbilityRune = true;
                break;
            case RunestoneType.Key:
                HasKeyRune = true;
                break;
            case RunestoneType.Blessing:
                HasBlessingRune = true;
                break;
        }
        OnRunestoneAcquired.Invoke(runestone);
    }
}
