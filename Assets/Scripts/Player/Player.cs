using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Tooltip interactTooltipPrefab;

    public UnityEvent<List<Artifact>> OnArtifactsChanged { get; private set; }

    private List<Artifact> artifacts = new List<Artifact>();
    private Vector2 movement;
    private float maxPlayerDist;
    private Transform otherPlayer;

    // interaction
    private Collider2D[] interactTargets = new Collider2D[5];
    private int numInteractTargets = 0;
    private Interactable targetInteractable;
    private Tooltip interactTooltip;

    private Rigidbody2D rb2D;
    private Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        OnArtifactsChanged = new UnityEvent<List<Artifact>>();
    }

    void Start()
    {
        interactTooltip = Instantiate(interactTooltipPrefab, transform.position, Quaternion.identity, HUD.TooltipParent);
        interactTooltip.SetTarget(transform);
        interactTooltip.SetActive(false);

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

        UpdateInteractions();
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        // hack cause Unity calls this on prefabs - WTF?
        if (!gameObject.activeInHierarchy) return;

        if (context.started && targetInteractable != null)
        {
            targetInteractable.Interact(this);
        }
    }

    public void AddArtifact(Artifact artifact)
    {
        artifacts.Add(artifact);
        OnArtifactsChanged.Invoke(artifacts);
    }

    private void UpdateInteractions()
    {
        Interactable interactable;

        // Clear current interactions
        for (int i = 0; i < numInteractTargets; i++)
        {
            if (interactTargets[i].TryGetComponent<Interactable>(out interactable))
            {
                interactable.SetTooltipActive(false);
            }
        }
        targetInteractable = null;
        interactTooltip.SetActive(false);

        // Check for nearby interactables
        numInteractTargets = Physics2D.OverlapCircleNonAlloc(transform.position, interactRadius, interactTargets, interactLayer);
        int closestIdx = -1;
        float smallestSqrDist = float.MaxValue;
        for (int i = 0; i < numInteractTargets; i++)
        {
            float sqrDist = (interactTargets[i].transform.position - transform.position).sqrMagnitude;
            if (sqrDist < smallestSqrDist)
            {
                smallestSqrDist = sqrDist;
                closestIdx = i;
            }
        }

        // Set the nearest interactable active
        if (closestIdx >= 0 && interactTargets[closestIdx].TryGetComponent<Interactable>(out interactable))
        {
            targetInteractable = interactable;
            interactable.SetTooltipActive(true);
            interactTooltip.SetActive(true);
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
