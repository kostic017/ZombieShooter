using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private AudioSource pickupSound;

    private Rigidbody2D rb;
    private HealthBar healthBar;

    private Dictionary<WeaponType, Weapon> weapons;

    private Vector2 movement;
    private Vector2 mousePosition;

    private WeaponType currentWeapon;

    private float health = 100f;

    private void Start()
    {
        weapons = new();
        InitializeWeapon();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetHealth(health);
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weapons[currentWeapon].SetIsMoving(movement != Vector2.zero);

        if (Input.GetMouseButtonDown(0))
            Shoot();
        else if (Input.GetMouseButtonDown(1))
            Melee();

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            ChangeWeapon(-1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            ChangeWeapon(+1);

        if (Input.GetKeyUp(KeyCode.R) && weapons[currentWeapon] is RangedWeapon weapon)
            weapon.Reload();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);
        Vector2 lookDirection = mousePosition - rb.position;
        rb.rotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            var collectable = other.gameObject.GetComponent<WeaponCollectable>();

            if (!weapons.ContainsKey(collectable.type))
                weapons.Add(collectable.type, FindWeapon(collectable.type));
            if (weapons[collectable.type] is RangedWeapon weapon)
                weapon.AddAmmo(collectable.amount);

            pickupSound.Play();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Medkit"))
        {
            health = 100f;
            pickupSound.Play();
            healthBar.SetHealth(health);
            Destroy(other.gameObject);
        }
    }

    private void InitializeWeapon()
    {
        var weapon = FindObjectOfType<Weapon>();
        weapons.Add(weapon.type, weapon);
        currentWeapon = weapon.type;
    }

    private Weapon FindWeapon(WeaponType type)
    {
        foreach (var weapon in FindObjectsOfType<Weapon>(true))
            if (weapon.type == type)
                return weapon;
        return null;
    }

    private void ChangeWeapon(int value)
    {
        WeaponType newWeapon = currentWeapon + value;
        if (value > 0)
        {
            while (!weapons.ContainsKey(newWeapon))
            {
                ++newWeapon;
                if (newWeapon > WeaponType.Shotgun)
                    newWeapon = WeaponType.Knife;
            }
        }
        else
        {
            while (!weapons.ContainsKey(newWeapon))
            {
                --newWeapon;
                if (newWeapon < WeaponType.Knife)
                    newWeapon = WeaponType.Shotgun;
            }
        }
        if (weapons.ContainsKey(newWeapon))
        {
            weapons[currentWeapon].gameObject.SetActive(false);
            weapons[newWeapon].gameObject.SetActive(true);
            currentWeapon = newWeapon;
        }
    }

    private void Shoot()
    {
        if (weapons[currentWeapon] is RangedWeapon weapon)
            weapon.Shoot(mousePosition - rb.position);
    }

    private void Melee()
    {
        weapons[currentWeapon].Melee();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
