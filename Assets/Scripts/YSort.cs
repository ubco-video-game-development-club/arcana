using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField] private float layerGranularity = 1.0f;
    [SerializeField] private new SpriteRenderer renderer;

    void Start()
    {
        renderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * layerGranularity);
    }
}
