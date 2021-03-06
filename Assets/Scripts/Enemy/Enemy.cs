using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float patrolRadius = 10.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float avoidanceRadius = 2.0f;
    [SerializeField] private new Rigidbody2D rigidbody;

    private int health;

    private Rigidbody2D rb2D;
    private Vector2[] path = null;
    private int pathIndex = 0;
    private Collider2D[] avoidanceBuffer = new Collider2D[8];

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void FixedUpdate()
    {
        DoAvoidance();
        Patrol();
    }

    public bool TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        if (health == 0)
        {
            Die();
            return true;
        }
        return false;
    }

    public void Knockback(Vector2 force)
    {
        rb2D.AddForce(force, ForceMode2D.Impulse);
    }

    private void DoAvoidance()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, avoidanceRadius, avoidanceBuffer);
        for(int i = 0; i < count; i++)
        {
            if(avoidanceBuffer[i].transform == transform) continue;
            Vector2 dir = (transform.position - avoidanceBuffer[i].transform.position).normalized;
            rigidbody.AddForce(dir * moveSpeed);
        }
    }

    private void Patrol()
    {
        if(path == null)
        {
            Vector2 destination = (Vector2)transform.position + Random.insideUnitCircle * patrolRadius;
            path = GameManager.AStar.FindPath(transform.position, destination);
            pathIndex = 0;
        } else if(pathIndex < path.Length)
        {
            Vector2 destination = path[pathIndex];
            Debug.DrawLine(destination, transform.position);

            Vector2 d = destination - (Vector2)transform.position;
            Vector2 dir = d.normalized;
            Vector2 force = dir * moveSpeed;
            rigidbody.AddForce(force);

            if(d.sqrMagnitude < 1.0f) pathIndex++;
        } else path = null;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
