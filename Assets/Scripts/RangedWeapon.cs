using TMPro;
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
    private int spareBullets;

    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private int shotInOneShot = 1;

    [SerializeField]
    private float cooldown = 0.5f;

    [SerializeField]
    private float shootDamage;

    [SerializeField]
    private Transform shootingPoint;

    [SerializeField]
    private GameObject muzzleFlashPrefab;

    private int bulletsInMag;

    private float lastShot;

    private void Awake()
    {
        bulletsInMag = magazineSize;
    }

    private void Update()
    {
        ammoCounter.text = $"{bulletsInMag} / {spareBullets}";
        if (bulletsInMag <= 0 && spareBullets > 0)
            Reload();
    }

    public void Shoot(Vector2 direction)
    {
        if (bulletsInMag > 0)
        {
            if (Time.time - lastShot > cooldown)
            {
                animator.SetTrigger("Shoot");
                shootSound.Play();
                var muzzleFlash = Instantiate(muzzleFlashPrefab, shootingPoint);
                Destroy(muzzleFlash, 0.1f);

                bulletsInMag -= shotInOneShot;
                lastShot = Time.time;
                if (bulletsInMag < 0)
                    bulletsInMag = 0;

                RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, direction, range, LayerMask.GetMask("Attack Target"));
                if (hit)
                {
                    var zombie = hit.transform.GetComponent<Zombie>();
                    if (zombie) zombie.TakeDamage(shootDamage);
                }
            }
        }
        else
        {
            emptyMagSound.Play();
        }
    }

    public void Reload()
    {
        if (spareBullets > 0)
        {
            if (bulletsInMag < magazineSize)
            {
                spareBullets += bulletsInMag;

                if (spareBullets < magazineSize)
                {
                    bulletsInMag = spareBullets;
                    spareBullets = 0;
                }
                else
                {
                    bulletsInMag = magazineSize;
                    spareBullets -= magazineSize;
                }

                animator.SetTrigger("Reload");
                reloadSound.Play();
            }
        }
        else
        {
            emptyMagSound.Play();
        }
    }

    public void AddAmmo(int amount)
    {
        spareBullets += amount;
    }
}
