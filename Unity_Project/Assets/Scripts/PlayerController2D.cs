using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public Collider2D standingCollider;
    public Collider2D crouchCollider;

    public float jumpCutMultiplier = 0.5f;

    public bool canJump = true;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private bool isGrounded = true;
    private bool isCrouching = false;

    // Goo effect variables
    private float speedMultiplier = 1f;
    private float gooTimer = 0f;
    private float gooDuration = 0f;
    private bool gooDisablesJump = false;

    [Header("Goo Visual")]
    public Color gooColor = Color.green;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        originalColor = sr.color;

        standingCollider.enabled = true;
        crouchCollider.enabled = false;

        animator.Play("Idle");
    }

    void Update()
    {
        HandleGooEffect();
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

        float currentSpeed = (isCrouching ? moveSpeed * 0.5f : moveSpeed) * speedMultiplier;

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

    void HandleGooEffect()
    {
        if (gooTimer > 0)
        {
            gooTimer -= Time.deltaTime;

            if (gooTimer <= 0)
            {
                speedMultiplier = 1f;
                canJump = true;
                gooDisablesJump = false;

                // Restore original color
                sr.color = originalColor;
            }
        }
    }

    public void ApplyGooEffect(float slowMultiplier, float duration, bool disableJump)
    {
        speedMultiplier = slowMultiplier;

        gooDuration = duration;
        gooTimer = duration;

        gooDisablesJump = disableJump;

        if (disableJump)
            canJump = false;

        // Apply goo color
        sr.color = gooColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5)
        {
            isGrounded = true;
        }
    }
}