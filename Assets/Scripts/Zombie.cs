using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    [SerializeField]
    private float speed = 5f;

    private Transform target;

    private Vector2 movement;

    private Rigidbody2D rb;
    private Animator animator;
    private HealthBar healthBar;
    private SpriteRenderer spriteRenderer;

    private float health;

    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<Player>().transform;
        healthBar.SetHealth(maxHealth, maxHealth);
    }

    private void Update()
    {
        if (spriteRenderer.isVisible)
        {
            movement = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
        else
        {
            movement = Vector2.zero;
        }
        animator.SetBool("IsMoving", movement != Vector2.zero);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x, movement.y) * speed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health, maxHealth);
        if (health <= 0)
            Destroy(gameObject);
    }
}
