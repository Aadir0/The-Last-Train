using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1f;   // Movement speed
    [SerializeField] private Vector2 direction = Vector2.left; // Movement direction
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public float resetX = -15f;
    public float startX = 15f;
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if(transform.position.x <= spriteRenderer.bounds.size.x * -1)
        {
            transform.position += Vector3.right * spriteRenderer.bounds.size.x * 2f;
        }
    }
}