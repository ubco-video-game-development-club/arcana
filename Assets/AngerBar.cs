using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBar : MonoBehaviour
{
    [SerializeField] private RectTransform barFill;

    void Start()
    {
        GameManager.ProgressionSystem.OnAngerLevelIncreased.AddListener(UpdateDisplay);
    }

    private void UpdateDisplay(float angerLevel)
    {
        barFill.anchorMax = new Vector2(barFill.anchorMax.x, angerLevel);
    }
}
