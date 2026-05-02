using UnityEngine;

public class WitchMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 2f;

    [Header("Attack")]
    [SerializeField] private WitchAttack witchAttack;
    [SerializeField] private float attackCooldown = 2.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private float attackTimer;
    private bool isAttacking;
    private bool isCharging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (witchAttack == null)
            witchAttack = GetComponentInChildren<WitchAttack>();
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

        attackTimer = attackCooldown;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            StopMoving();
            return;
        }

        if (player == null)
        {
            StopMoving();
            return;
        }

        attackTimer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        FacePlayer();

        if (isAttacking)
        {
            StopMoving();
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            StopMoving();

            if (!isCharging && attackTimer >= attackCooldown)
            {
                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        if (anim != null)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
    }

    private void StopMoving()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        if (anim != null)
        {
            anim.SetBool("isAttacking", true);
            anim.SetBool("isWalking", false);
        }
    }

    private void FacePlayer()
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

    private void AttackPlayer()
    {
        isCharging = true;
        isAttacking = true;
        attackTimer = 0f;

        if (anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetTrigger("charge");
        }
    }

    public void ReleaseChargedAttack()
    {
        if (witchAttack == null || player == null) return;

        witchAttack.BeginAttack(player);
    }

    public void EndAttack()
    {
        isAttacking = false;
        isCharging = false;

        if (anim != null)
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("isCharging", false);
        }
    }

    public void BeginAttackState()
    {
        isCharging = true;

        if (anim != null)
        {
            anim.SetBool("isCharging", true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}