using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunestonePickup : Interactable
{
    [SerializeField] private Runestone runestone;

    protected override void OnTooltipCreated(Tooltip tooltip)
    {
        tooltip.SetText(runestone.Title + "\n" + runestone.Desc);
    }

    public override void Interact(Player player)
    {
        GameManager.ProgressionSystem.AcquireRunestone(runestone);
        Destroy(gameObject);
    }
}
