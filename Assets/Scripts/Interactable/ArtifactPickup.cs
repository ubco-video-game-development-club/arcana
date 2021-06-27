using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickup : Interactable
{
    [SerializeField] private Artifact artifact;

    protected override void OnTooltipCreated(Tooltip tooltip)
    {
        tooltip.SetText(artifact.Title + "\n" + artifact.Desc);
    }
}
