using UnityEngine;

public class GooJunior : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float hopForce = 5f;

    [Header("Hop Timing")]
    public float minHopCooldown = 1f;
    public float maxHopCooldown = 2f;

    private float hopTimer;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isGrounded;

    public LayerMask groundLayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        SetNextHopTime();
    }

    void Update()
    {
        if (player == null) return;

        hopTimer -= Time.deltaTime;

        // Flip sprite
        sr.flipX = player.position.x < transform.position.x;

        // Hop toward player
        if (hopTimer <= 0f && isGrounded)
        {
            HopTowardsPlayer();
            SetNextHopTime();
        }
    }

    void SetNextHopTime()
    {
        hopTimer = Random.Range(minHopCooldown, maxHopCooldown);
    }

    void HopTowardsPlayer()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed, hopForce);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground check
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }

        // Ignore other slimes
        if (collision.gameObject.CompareTag("Googene"))
        {
            Physics2D.IgnoreCollision(
                collision.collider,
                GetComponent<Collider2D>()
            );
            return;
        }

        // Player interaction
        if (collision.gameObject.CompareTag("Player"))
        {
            bool stomped = false;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Player hit from above
                if (contact.normal.y < -0.5f)
                {
                    stomped = true;
                    break;
                }
            }

            if (stomped)
            {
                // Bounce player
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 6f);
                }

                // Destroy slime
                Destroy(gameObject);
            }
            else
            {
                // Kill player ONLY if not stomped
                collision.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}