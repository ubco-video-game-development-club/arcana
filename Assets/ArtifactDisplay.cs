using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDisplay : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Tooltip tooltip;

    public void SetArtifact(Artifact artifact)
    {
        iconImage.sprite = artifact.HudIcon;
        tooltip.SetText(artifact.Title + "\n" + artifact.Desc);
        tooltip.SetActive(false);
    }

    public void SetTootipActive(bool active)
    {
        tooltip.SetActive(active);
    }
}
