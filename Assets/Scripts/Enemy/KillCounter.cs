using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter instance;

    public int enemiesKilled = 0;
    public int score = 0;

    void Awake()
    {
        instance = this;
    }

    public void EnemyKilled(int points)
    {
        enemiesKilled++;
        score += points;
    }
}