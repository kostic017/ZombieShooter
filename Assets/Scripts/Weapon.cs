using UnityEngine;

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

    public void SetIsMoving(bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);
    }

    public void Melee()
    {
        animator.SetTrigger("Melee");
        var zombie = Physics2D.OverlapCircle(meleePoint.position, meleeRange, LayerMask.GetMask("Zombies"));
        if (zombie != null)
        {
            meleeHitSound.Play();
            zombie.GetComponent<Zombie>().TakeDamage(meleeDamage);
        }
        else
        {
            meleeWhooshSound.Play();
        }
    }
}
