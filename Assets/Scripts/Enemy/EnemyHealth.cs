using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth { get; private set; }
    [Header("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private Behaviour[] components;
    private Animator anim;
    public bool dead;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private int points = 10;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dieSfx;
    private SkeletonMovement skeletonMovement;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        skeletonMovement = GetComponent<SkeletonMovement>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float _damage)
    {
        if (dead) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            bool alreadyStunned = skeletonMovement != null && skeletonMovement.IsStunned();

            if (!alreadyStunned)
            {
                anim.ResetTrigger("hurt");
                anim.SetTrigger("hurt");
                skeletonMovement.StunMovement(1f);
            }
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        if (!dead)
        {
            dead = true;

            anim.SetTrigger("die");
            PlayDieSfx();

            foreach (Behaviour component in components)
                component.enabled = false;

            KillCounter.instance.EnemyKilled(points);
            Destroy(Enemy, 1f);
        }
    }

    private void PlayDieSfx()
    {
        if (dieSfx == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(dieSfx);
            return;
        }

        AudioSource.PlayClipAtPoint(dieSfx, transform.position);
    }
}