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
        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("moving", isMoving);
        if (isMoving)
        {
            Vector2 moveDir = movement.normalized;
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
        Debug.Log("Working");

        // hack cause Unity calls this on prefabs - WTF?
        if (!gameObject.activeInHierarchy) return;
        
        movement = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // hack cause Unity calls this on prefabs - WTF?
        if (!gameObject.activeInHierarchy) return;

        if (context.started)
        {
            animator.SetTrigger("attack");
        }
    }
}
