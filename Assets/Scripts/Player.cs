using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Vector2 movement;

    private Rigidbody2D rb2D;
    private Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 moveDir = movement.normalized;
        if (moveDir.sqrMagnitude > 0)
        {
            animator.SetFloat("xDir", moveDir.x);
            animator.SetFloat("yDir", moveDir.y);
        }
    }

    void FixedUpdate()
    {
        rb2D.AddForce(movement * moveSpeed);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
}
