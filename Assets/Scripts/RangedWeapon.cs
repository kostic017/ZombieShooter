using System;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    private AudioSource shootSound;

    [SerializeField]
    private AudioSource reloadSound;

    [SerializeField]
    private AudioSource emptyMagSound;

    [SerializeField]
    private int magazineSize;

    [SerializeField]
    private int totalBullets;

    [SerializeField]
    private int shotInOneShot = 1;

    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private float shootDamage;

    private int bulletsInMag;

    private void Awake()
    {
        bulletsInMag = magazineSize;
    }

    public void Shoot(Vector2 direction)
    {
        if (bulletsInMag > 0)
        {
            animator.SetTrigger("Shoot");
            shootSound.Play();
            bulletsInMag -= shotInOneShot;
            totalBullets -= shotInOneShot;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, LayerMask.GetMask("Zombies"));
            if (hit)
                hit.transform.GetComponent<Zombie>().TakeDamage(shootDamage);
        }
        else
        {
            emptyMagSound.Play();
        }
    }

    public void Reload()
    {
        if (totalBullets > 0)
        {
            if (bulletsInMag < magazineSize)
            {
                animator.SetTrigger("Reload");
                reloadSound.Play();
                bulletsInMag = magazineSize;
                totalBullets -= magazineSize;
            }
        }
        else
        {
            emptyMagSound.Play();
        }
    }

    public void AddAmmo(int amount)
    {
        totalBullets += amount;
    }
}
