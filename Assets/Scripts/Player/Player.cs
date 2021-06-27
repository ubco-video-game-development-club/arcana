using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private PlayerAttack attack;

    private Vector2 movement;
    private float maxPlayerDist;
    private Transform otherPlayer;

    private Rigidbody2D rb2D;
    private Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        otherPlayer = GameManager.GetOtherPlayer(this).transform;
        maxPlayerDist = Camera.main.orthographicSize * 2 - 2;

        attack.Enabled = true;
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
        Vector2 moveForce = movement * moveSpeed;

        // Prevent moving too far away
        Vector2 playerDiff = transform.position - otherPlayer.position;
        bool isPastLimit = playerDiff.sqrMagnitude > maxPlayerDist * maxPlayerDist;
        if (isPastLimit && Vector2.Dot(playerDiff, moveForce) > 0) return;

        rb2D.AddForce(moveForce);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // hack cause Unity calls this on prefabs - WTF?
        if (!gameObject.activeInHierarchy) return;

        movement = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // hack cause Unity calls this on prefabs - WTF?
        if (!gameObject.activeInHierarchy) return;

        if (context.started && attack.Enabled)
        {
            animator.SetTrigger("attack");
            attack.Invoke(CreateAttackData());
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        attack.Enabled = false;
        yield return new WaitForSeconds(attack.Cooldown);
        attack.Enabled = true;
    }

    private AttackData CreateAttackData()
    {
        AttackData data = new AttackData();
        data.origin = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        data.direction = (mousePos - data.origin).normalized;
        // TODO: read damage mods from artifacts here
        return data;
    }
}
