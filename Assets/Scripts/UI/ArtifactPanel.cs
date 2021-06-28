using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPanel : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ArtifactDisplay artifactDisplayPrefab;

    private List<ArtifactDisplay> displays;

    void Start()
    {
        displays = new List<ArtifactDisplay>();
        player.OnArtifactsChanged.AddListener(UpdateDisplay);
    }

    private void UpdateDisplay(List<Artifact> artifacts)
    {
        // Clear existing displays
        foreach (ArtifactDisplay display in displays)
        {
            Destroy(display, 0);
        }

        // Add new artifacts
        float xPos = 0;
        for (int i = 0; i < artifacts.Count; i++)
        {
            ArtifactDisplay display = Instantiate(artifactDisplayPrefab, transform);
            RectTransform rt = display.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(xPos, 0);
            display.SetArtifact(artifacts[i]);
            displays.Add(display);
            xPos += rt.rect.width;
        }
    }
}
