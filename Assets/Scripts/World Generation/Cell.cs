using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    public void SetSprite(Sprite sprite)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = "Background";
        renderer.sprite = sprite;
    }
}
