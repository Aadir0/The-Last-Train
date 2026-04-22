using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth { get; private set; }
    [Header("Health")]
    [SerializeField] private Behaviour[] components;
    public float startingHealth;
    [SerializeField] private GameObject deathUI;
    private Animator anim;
    public bool dead;
    [SerializeField] private GameObject player;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dieSfx;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float _damage)
    {
        if (dead) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
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
            Cursor.visible = true;

            anim.SetTrigger("die");
            PlayDieSfx();
            deathUI.SetActive(true);

            foreach (Behaviour component in components)
                component.enabled = false;
            
            Destroy(player, 1f);
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