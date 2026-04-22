using System;
using ForestBackgroundsPixelArt;
using Unity.VisualScripting;
using UnityEngine;

public class AnimStater : MonoBehaviour
{
    [SerializeField] private Animator deathanim;
    [SerializeField] private PlayerHealth playerHealth;
    void Update()
    {
        if (playerHealth.dead)
        {
            deathanim.SetTrigger("move");
        }
    }
}
