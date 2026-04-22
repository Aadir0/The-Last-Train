using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSfx;
    private bool isGrounded;
    private bool moveRight = true;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            anim.SetBool("run", false);
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput > 0.01f && !moveRight)
        {
            Flip();
        }
        else if (moveInput < -0.01f && moveRight)
        {
            Flip();
        }

        anim.SetBool("run", Mathf.Abs(moveInput) > 0.01f);
        anim.SetBool("grounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        anim.SetTrigger("jump");
        PlayJumpSfx();
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;
    }

    void PlayJumpSfx()
    {
        if (jumpSfx == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(jumpSfx);
            return;
        }

        AudioSource.PlayClipAtPoint(jumpSfx, transform.position);
    }

    void Flip()
    {
        moveRight = !moveRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
