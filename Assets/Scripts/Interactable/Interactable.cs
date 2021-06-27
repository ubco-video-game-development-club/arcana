using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Tooltip tooltipPrefab;

    private Tooltip tooltip;

    void Start()
    {
        tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity, HUD.TooltipParent);
        tooltip.SetTarget(transform);
        tooltip.SetActive(false);
        OnTooltipCreated(tooltip);
    }

    public void SetTooltipActive(bool active)
    {
        tooltip.SetActive(active);
    }

    protected abstract void OnTooltipCreated(Tooltip tooltip);
}
