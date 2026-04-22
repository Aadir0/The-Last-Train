using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private Image totalhealthBar;

    [SerializeField] private Image currenthealthBar;

    [SerializeField, Range(0f, 1f)] private float maxFillAmount = 0.3f;

    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("Player Health not assigned to Healthbar! Please assign it in the Inspector.");
            return;
        }
        
        totalhealthBar.fillAmount = maxFillAmount;
        currenthealthBar.fillAmount = maxFillAmount;
    }
    
    private void Update()
    {
        if (playerHealth == null)
        {
            return;
        }

        currenthealthBar.fillAmount = (playerHealth.currentHealth / playerHealth.startingHealth) * maxFillAmount;
    }
}