using System.Collections;
using UnityEngine;

public class WitchAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float activeDuration = 0.8f;
    [SerializeField] private float damageDelay = 0.35f;
    [SerializeField] private float feetOffsetY = -0.08f;
    [SerializeField] private float airborneMissHeight = 0.25f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSfx;

    private Animator anim;
    private Collider2D attackCollider;
    private Transform player;
    private bool isAttacking;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        attackCollider = GetComponent<Collider2D>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    public void BeginAttack(Transform targetPlayer)
    {
        player = targetPlayer;

        Vector3 footPosition = GetPlayerFeetPosition(targetPlayer);
        transform.position = new Vector3(footPosition.x, footPosition.y + feetOffsetY, transform.position.z);

        if (attackCollider != null)
            attackCollider.enabled = true;

        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        PlayAttackSfx();

        if (anim != null)
        {
            anim.SetTrigger("attack");
        }

        yield return new WaitForSeconds(damageDelay);
        DealDamage();

        yield return new WaitForSeconds(Mathf.Max(0f, activeDuration - damageDelay));

        if (attackCollider != null)
            attackCollider.enabled = false;

        isAttacking = false;

        WitchMovement witchMovement = GetComponentInParent<WitchMovement>();
        if (witchMovement != null)
        {
            witchMovement.EndAttack();
        }

        Destroy(gameObject);
    }

    public void DealDamage()
    {
        if (player == null) return;

        if (IsPlayerAirborne()) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"Witch dealt {damage} damage to player!");
        }
    }

    private Vector3 GetPlayerFeetPosition(Transform targetPlayer)
    {
        if (targetPlayer == null) return transform.position;

        Collider2D playerCollider = targetPlayer.GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Vector3 boundsMin = playerCollider.bounds.min;
            return new Vector3(boundsMin.x + playerCollider.bounds.size.x * 0.5f, boundsMin.y, targetPlayer.position.z);
        }

        return targetPlayer.position;
    }

    private bool IsPlayerAirborne()
    {
        if (player == null) return false;

        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider == null) return false;

        return playerCollider.bounds.min.y > transform.position.y + airborneMissHeight;
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
