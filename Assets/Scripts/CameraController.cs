using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float followTime = 0.2f;

    private Transform target1;
    private Transform target2;
    private Vector2 refVelocity;

    void Start()
    {
        target1 = GameManager.Player1;
        target2 = GameManager.Player2;
    }

    void FixedUpdate()
    {
        Vector2 targetPos = (GameManager.Player1.position + GameManager.Player2.position) / 2;
        Vector2 newPos = Vector2.SmoothDamp(transform.position, targetPos, ref refVelocity, followTime);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
