using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Weapon[] weapons;

    private Rigidbody2D rb;

    private int currentWeapon;
    private int unlockedWeapons = 1;

    private Vector2 movement;
    private Vector2 mousePosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            ChangeWeapon(currentWeapon + 1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            ChangeWeapon(currentWeapon - 1);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    private void ChangeWeapon(int weapon)
    {
        if (weapon >= unlockedWeapons) weapon = 0;
        if (weapon < 0) weapon = unlockedWeapons - 1;
        weapons[currentWeapon].gameObject.SetActive(false);
        weapons[weapon].gameObject.SetActive(true);
        currentWeapon = weapon;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            unlockedWeapons++;
            Destroy(other.gameObject);
        }
    }
}
