using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunestoneDisplay : MonoBehaviour
{
    [SerializeField] private Runestone runestone;
    public Runestone Runestone { get => runestone; }

    [SerializeField] private Image iconImage;
    [SerializeField] private Tooltip tooltip;

    private bool isRunestoneActive = false;

    void Start()
    {
        iconImage.sprite = runestone.InactiveHudIcon;
        tooltip.SetText(runestone.Title + "\n" + runestone.Desc);
        tooltip.SetActive(false);
    }

    public void SetRunestoneActive()
    {
        iconImage.sprite = runestone.ActiveHudIcon;
        isRunestoneActive = true;
    }

    public void SetTootipActive(bool active)
    {
        if (!isRunestoneActive) return;
        tooltip.SetActive(active);
    }
}
