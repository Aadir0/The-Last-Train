using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private int damage1 = 1;
    [SerializeField] private int damage2 = 2;
    [SerializeField] private float range = 1;
    [SerializeField] private float attackCooldown1;
    [SerializeField] private float attackCooldown2;
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private PolygonCollider2D polygonCollider2D;
    [Header("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attack1Sfx;
    [SerializeField] private AudioClip attack2Sfx;
    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown1)
        {
            Attack();
        }

        if (Input.GetMouseButtonDown(1) && cooldownTimer > attackCooldown2)
        {
            Attack2();
        }
    }

    private void Attack()
    {
        anim.SetBool("endAttack", false);
        anim.SetTrigger("attack");
        PlayAttackSfx(attack1Sfx);
        cooldownTimer = 0;
    }

    private void Attack2()
    {
        anim.SetBool("endAttack", false);
        anim.SetTrigger("attack2");
        PlayAttackSfx(attack2Sfx);
        cooldownTimer = 0;
    }

    private void PlayAttackSfx(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
            return;
        }

        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    public void endAttack()
    {
        anim.SetBool("endAttack", true);
    }

    public void DamageEnemy()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            polygonCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(polygonCollider2D.bounds.size.x * range, polygonCollider2D.bounds.size.y, polygonCollider2D.bounds.size.z),
            0,
            Vector2.left,
            0,
            enemyLayer
        );

        if (hit.collider != null)
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    enemyHealth.TakeDamage(damage1);
                    Debug.Log($"Player dealt {damage1} damage to enemy!");
                }
                else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                {
                    enemyHealth.TakeDamage(damage2);
                    Debug.Log($"Player dealt {damage2} damage to enemy!");
                }
            }
            
            Button uiButton = hit.collider.GetComponent<Button>();
            if (uiButton != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                uiButton.onClick.Invoke();
                Debug.Log("UI Button clicked by player attack!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            polygonCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(polygonCollider2D.bounds.size.x * range, polygonCollider2D.bounds.size.y, polygonCollider2D.bounds.size.z)
        );
    }
}