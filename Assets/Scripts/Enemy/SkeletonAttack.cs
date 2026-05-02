using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    public float attackCooldown = 2.5f;
    [SerializeField] private float attackDuration = 0.8f;
    [SerializeField] private int damage = 1;
    private Transform player;
    private Animator anim;
    private SkeletonMovement skeletonMovement;
    private float attackTimer;
    private float attackStateTimer;
    private bool isAttacking = false;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSfx;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        skeletonMovement = GetComponent<SkeletonMovement>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        attackTimer = attackCooldown;
    }
    
    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (player == null && skeletonMovement != null)
        {
            player = skeletonMovement.GetPlayer();
        }
        
        if (player == null) return;
        
        attackTimer += Time.deltaTime;

        if (isAttacking)
        {
            attackStateTimer += Time.deltaTime;
            if (attackStateTimer >= attackDuration)
            {
                EndAttack();
            }
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange && attackTimer >= attackCooldown && !isAttacking)
        {
            AttackPlayer();
        }
    }
    
    private void AttackPlayer()
    {
        isAttacking = true;
        attackTimer = 0;
        attackStateTimer = 0;
        
        if (anim != null)
        {
            anim.SetTrigger("attack");
            PlayAttackSfx();
        }
        else
        {
            DealDamage();
        }
    }
    public void DealDamage()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Skeleton dealt {damage} damage to player!");
            }
        }
    }
    
    public void EndAttack()
    {
        if (!isAttacking) return;
        isAttacking = false;
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
    }
    
    public float GetAttackRange()
    {
        return attackRange;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void PlayAttackSfx()
    {
        if (attackSfx == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(attackSfx);
            return;
        }

        AudioSource.PlayClipAtPoint(attackSfx, transform.position);
    }
}
