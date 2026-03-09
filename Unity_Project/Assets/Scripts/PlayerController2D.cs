using UnityEngine;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public Collider2D standingCollider;
    public Collider2D crouchCollider;

    public float jumpCutMultiplier = 0.5f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        standingCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    void Update()
    {
        Crouch();
        Move();
        Jump();
        BetterJump();
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

        float currentSpeed = isCrouching ? moveSpeed * 0.5f : moveSpeed;

        rb.linearVelocity = new Vector2(move * currentSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void BetterJump()
    {
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (!isCrouching)
            {
                standingCollider.enabled = false;
                crouchCollider.enabled = true;
                isCrouching = true;
            }
        }
        else
        {
            if (isCrouching)
            {
                standingCollider.enabled = true;
                crouchCollider.enabled = false;
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