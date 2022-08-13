using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    private AudioSource shootSound;

    [SerializeField]
    private AudioSource reloadSound;

    [SerializeField]
    private int maxBullets;

    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private float shootDamage;

    private int bullets;

    private void Awake()
    {
        bullets = maxBullets;
    }

    public void Shoot(Vector2 direction)
    {
        if (bullets <= 0)
        {
            animator.SetTrigger("Reload");
            reloadSound.Play();
            bullets = maxBullets;
        }

        animator.SetTrigger("Shoot");
        shootSound.Play();
        --bullets;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, LayerMask.GetMask("Zombies"));
        if (hit)
            hit.transform.GetComponent<Zombie>().TakeDamage(shootDamage);
    }
}
