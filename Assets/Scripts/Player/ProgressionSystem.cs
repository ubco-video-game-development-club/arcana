using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressionSystem : MonoBehaviour
{
    [SerializeField] private float initialAngerTickDelay = 10f;
    [SerializeField] private float angerGrowthPerMinute = 0.0125f;
    [SerializeField] private float angerGrowthPerRunestone = 0.2f;

    public bool HasBuffRune { get; private set; }
    public bool HasAbilityRune { get; private set; }
    public bool HasBlessingRune { get; private set; }
    public bool HasKeyRune { get; private set; }

    public UnityEvent<Runestone> OnRunestoneAcquired { get; private set; }
    public UnityEvent<float> OnAngerLevelIncreased { get; private set; }

    private List<Runestone> runestones = new List<Runestone>();
    private float angerLevel = 0;

    private YieldInstruction waitForOneMinute;

    void Awake()
    {
        OnRunestoneAcquired = new UnityEvent<Runestone>();
        OnAngerLevelIncreased = new UnityEvent<float>();
        waitForOneMinute = new WaitForSeconds(60);
    }

    void Start()
    {
        StartCoroutine(AngerGrowthTick());
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

        angerLevel = Mathf.Min(1f, angerLevel + angerGrowthPerRunestone);
        OnAngerLevelIncreased.Invoke(angerLevel);
    }

    private IEnumerator AngerGrowthTick()
    {
        yield return new WaitForSeconds(initialAngerTickDelay);
        while (angerLevel <= 1f)
        {
            angerLevel = Mathf.Min(1f, angerLevel + angerGrowthPerMinute);
            OnAngerLevelIncreased.Invoke(angerLevel);
            yield return waitForOneMinute;
        }
    }
}
