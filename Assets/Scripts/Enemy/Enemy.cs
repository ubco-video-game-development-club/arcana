using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;

    private int health;

    private Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        health = maxHealth;
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

    private void Die()
    {
        Destroy(gameObject);
    }
}
