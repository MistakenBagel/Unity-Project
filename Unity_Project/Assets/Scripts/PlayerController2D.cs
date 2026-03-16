using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public Collider2D standingCollider;
    public Collider2D crouchCollider;

    public float jumpCutMultiplier = 0.5f;

    public bool canJump = true; // Allows external scripts (like GooPuddle) to disable jumping

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded = true;
    private bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        standingCollider.enabled = true;
        crouchCollider.enabled = false;

        animator.Play("Idle");
    }

    void Update()
    {
        Crouch();
        Move();
        Jump();
        BetterJump();
        UpdateAnimation();
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 0.5f, 9.5f);
        transform.position = pos;
    }

    void Move()
    {
        float move = 0;

        if (Input.GetKey(KeyCode.A))
            move = -1;

        if (Input.GetKey(KeyCode.D))
            move = 1;

        float currentSpeed = isCrouching ? moveSpeed * 0.5f : moveSpeed;

        rb.linearVelocity = new Vector2(move * currentSpeed, rb.linearVelocity.y);

        if (move != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (move > 0 ? 1 : -1);
            transform.localScale = scale;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;

            animator.Play("Jump");
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

                animator.SetBool("isCrouched", true);
            }
        }
        else
        {
            if (isCrouching)
            {
                standingCollider.enabled = true;
                crouchCollider.enabled = false;
                isCrouching = false;

                animator.SetBool("isCrouched", false);
            }
        }
    }

    void UpdateAnimation()
    {
        if (!isGrounded)
            return;

        float speed = Mathf.Abs(rb.linearVelocity.x);

        if (isCrouching)
        {
            if (speed > 0.1f)
            {
                animator.Play("Crawl");
            }
            else
            {
                animator.Play("Crouch");
            }
            return;
        }

        if (speed > 0.1f)
        {
            animator.Play("Walk");
        }
        else
        {
            animator.Play("Idle");
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