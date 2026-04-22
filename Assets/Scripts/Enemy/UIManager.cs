using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = 
            "Score: " + KillCounter.instance.score +
            "\nKills: " + KillCounter.instance.enemiesKilled;
    }
}