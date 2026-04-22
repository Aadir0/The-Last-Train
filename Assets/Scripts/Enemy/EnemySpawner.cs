using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float spawnRate = 2f;
    public int maxEnemies = 7;

    private float timer;

    void Start()
    {
        InvokeRepeating(nameof(IncreaseDifficulty), 20f, 5f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            TrySpawnEnemy();
            timer = 0f;
        }
    }

    void TrySpawnEnemy()
    {
        // Count enemies using tag (NO other scripts needed)
        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (currentEnemies >= maxEnemies)
            return;

        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform point = spawnPoints[index];

        Instantiate(enemyPrefab, point.position, point.rotation);
    }

    void IncreaseDifficulty()
    {
        spawnRate -= 0.1f;
        spawnRate = Mathf.Clamp(spawnRate, 4f, 6f);
    }
}