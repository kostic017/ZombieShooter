using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float damageDealt = 10f;

    [SerializeField]
    private GameObject dropsItem;

    private Player player;
    private Rigidbody2D rb;
    private Animator animator;
    private HealthBar healthBar;
    private GameManager gameManager;

    private Vector2 movement;

    private bool active;
    
    private float health;

    private void Start()
    {
        health = 100f;
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetHealth(health);
    }

    private void Update()
    {
        if (active)
        {
            if (!IsAttacking())
            {
                animator.SetBool("IsMoving", true);
                movement = (player.transform.position - transform.position).normalized;
            }
        }
        else
        {
            movement = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x, movement.y) * speed;
        rb.rotation = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!IsAttacking() && other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
            StartCoroutine(DealDamage());
        }
    }

    private void OnBecameVisible()
    {
        active = true;
    }

    private void OnBecameInvisible()
    {
        active = false;
    }

    private IEnumerator DealDamage()
    {
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        player.TakeDamage(damageDealt);
    }

    private bool IsAttacking()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            if (dropsItem != null)
                Instantiate(dropsItem, transform.position, Quaternion.identity);
            gameManager.ZombieKilled();
            Destroy(gameObject);
        }
    }
}
