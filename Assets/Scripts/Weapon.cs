using TMPro;
using UnityEngine;

public enum WeaponType
{
    Knife,
    Handgun,
    Rifle,
    Shotgun
}

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float meleeRange;
    
    [SerializeField]
    private float meleeDamage;
    
    [SerializeField]
    private Transform meleePoint;
    
    [SerializeField]
    private AudioSource meleeHitSound;
    
    [SerializeField]
    private AudioSource meleeWhooshSound;

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    private WeaponType type;

    [SerializeField]
    protected TextMeshProUGUI ammoCounter;

    public WeaponType Type => type;

    public Animator Animator => animator;

    private void Update()
    {
        ammoCounter.text = "N/A";
    }

    public void Melee()
    {
        animator.SetTrigger("Melee");
        var hit = Physics2D.OverlapCircle(meleePoint.position, meleeRange, LayerMask.GetMask("Attack Target"));
        if (hit)
        {
            meleeHitSound.Play();
            var zombie = hit.GetComponent<Zombie>();
            if (zombie) zombie.TakeDamage(meleeDamage);
        }
        else
        {
            meleeWhooshSound.Play();
        }
    }
}
