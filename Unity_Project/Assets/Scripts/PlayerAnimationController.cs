using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    public float moveThreshold = 0.1f;
    public float airThreshold = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleAnimations();
        HandleSpriteFlip();
    }

    void HandleAnimations()
    {
        float xVelocity = rb.linearVelocity.x;
        float yVelocity = rb.linearVelocity.y;

        bool isMoving = Mathf.Abs(xVelocity) > moveThreshold;
        bool isInAir = Mathf.Abs(yVelocity) > airThreshold;
        bool isCrouching = Input.GetKey(KeyCode.S);

        animator.SetBool("isWalking", isMoving && !isInAir);
        animator.SetBool("isJumping", isInAir);
        animator.SetBool("isCrouching", isCrouching);
    }

    void HandleSpriteFlip()
    {
        float xVelocity = rb.linearVelocity.x;

        if (xVelocity < -0.1f)
        {
            sprite.flipX = true;
        }
        else if (xVelocity > 0.1f)
        {
            sprite.flipX = false;
        }
    }
}