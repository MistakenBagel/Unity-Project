using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float crouchScale = 0.5f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private Vector3 originalScale;
    private bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        Move();
        Jump();
        Crouch();
    }

    void Move()
    {
        float move = 0;

        if (Input.GetKey(KeyCode.A))
        {
            move = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move = 1;
        }

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (!isCrouching)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScale, originalScale.z);
                isCrouching = true;
            }
        }
        else
        {
            if (isCrouching)
            {
                transform.localScale = originalScale;
                isCrouching = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5)
        {
            isGrounded = true;
        }
    }
}