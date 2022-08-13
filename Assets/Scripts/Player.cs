using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Weapon[] weapons;

    [SerializeField]
    private int unlockedWeapons = 2;

    [SerializeField]
    private WeaponType currentWeapon;

    private Rigidbody2D rb;
    private HealthBar healthBar;

    private Vector2 movement;
    private Vector2 mousePosition;

    private float health = 100f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapons[(int)currentWeapon].gameObject.SetActive(true);
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetHealth(health);
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weapons[(int)currentWeapon].SetIsMoving(movement != Vector2.zero);

        if (Input.GetMouseButtonDown(0))
            Shoot();
        else if (Input.GetMouseButtonDown(1))
            Melee();

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            ChangeWeapon((int)currentWeapon - 1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            ChangeWeapon((int)currentWeapon + 1);

        if (Input.GetKeyUp(KeyCode.R) && weapons[(int)currentWeapon] is RangedWeapon weapon)
            weapon.Reload();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);
        Vector2 lookDirection = mousePosition - rb.position;
        rb.rotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            unlockedWeapons++;
            Destroy(other.gameObject);
        }
    }

    private void ChangeWeapon(int weapon)
    {
        if (weapon >= unlockedWeapons) weapon = 0;
        if (weapon < 0) weapon = unlockedWeapons - 1;
        weapons[(int)currentWeapon].gameObject.SetActive(false);
        weapons[weapon].gameObject.SetActive(true);
        currentWeapon = (WeaponType)weapon;
    }

    private void Shoot()
    {
        if (weapons[(int)currentWeapon] is RangedWeapon weapon)
            weapon.Shoot(mousePosition - rb.position);
    }

    private void Melee()
    {
        weapons[(int)currentWeapon].Melee();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
