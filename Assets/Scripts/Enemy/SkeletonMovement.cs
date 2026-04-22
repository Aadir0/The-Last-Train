using UnityEngine;
using System.Collections;

public class SkeletonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseRange = 10f;
    
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private SkeletonAttack skeletonAttack;
    private bool isStunned = false;
    private Coroutine stunCoroutine;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        skeletonAttack = GetComponent<SkeletonAttack>();
    }
    
    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the player has the 'Player' tag.");
        }
    }
    
    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            StopMoving();
            return;
        }

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Baundries"), true);
        if (player == null || isStunned) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        bool isAttacking = skeletonAttack != null && skeletonAttack.IsAttacking();
        float attackRange = skeletonAttack != null ? skeletonAttack.GetAttackRange() : 0f;
        
        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange && !isAttacking)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
        
        if (distanceToPlayer <= chaseRange)
        {
            FlipTowardsPlayer();
        }
    }
    
    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        
        if (anim != null)
        {
            anim.SetBool("isWalking", true);
        }
    }
    
    private void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        if (anim != null)
        {
            anim.SetBool("isWalking", false);
        }
    }
    
    private void FlipTowardsPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    
    public Transform GetPlayer()
    {
        return player;
    }

    public bool IsStunned()
    {
        return isStunned;
    }
    
    public void StunMovement(float duration)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }

        stunCoroutine = StartCoroutine(StunCoroutine(duration));
    }
    
    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        StopMoving();
        yield return new WaitForSeconds(duration);
        isStunned = false;
        stunCoroutine = null;
        RefreshMovementState();
    }

    private void RefreshMovementState()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isAttacking = skeletonAttack != null && skeletonAttack.IsAttacking();
        float attackRange = skeletonAttack != null ? skeletonAttack.GetAttackRange() : 0f;

        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange && !isAttacking)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
