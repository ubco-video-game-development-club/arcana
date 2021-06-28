using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunestonePanel : MonoBehaviour
{
    [SerializeField] private RunestoneDisplay[] runestoneDisplays;

    void Start()
    {
        GameManager.ProgressionSystem.OnRunestoneAcquired.AddListener(UpdateDisplay);
    }

    private void UpdateDisplay(Runestone runestone)
    {
        foreach (RunestoneDisplay display in runestoneDisplays)
        {
            if (display.Runestone.RunestoneType == runestone.RunestoneType)
            {
                display.SetRunestoneActive();
            }
        }
    }
}
