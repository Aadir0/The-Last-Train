using UnityEngine;

public class TrainWhistleOnKills : MonoBehaviour
{
    [Header("Kill Threshold")]
    [SerializeField] private int killsPerWhistle = 10;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip trainWhistleClip;

    private int nextWhistleAt;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (killsPerWhistle < 1)
            killsPerWhistle = 1;

        nextWhistleAt = killsPerWhistle;
    }

    private void Update()
    {
        if (KillCounter.instance == null) return;

        if (KillCounter.instance.enemiesKilled >= nextWhistleAt)
        {
            PlayWhistle();

            while (KillCounter.instance.enemiesKilled >= nextWhistleAt)
            {
                nextWhistleAt += killsPerWhistle;
            }
        }
    }

    private void PlayWhistle()
    {
        if (trainWhistleClip == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(trainWhistleClip);
            return;
        }

        AudioSource.PlayClipAtPoint(trainWhistleClip, transform.position);
    }
}
