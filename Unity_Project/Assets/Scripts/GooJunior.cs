using UnityEngine;

public class GooJunior : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float hopForce = 5f;

    [Header("Hop Timing")]
    public float minHopCooldown = 1f;
    public float maxHopCooldown = 2f;

    [Header("Effects")]
    public GameObject destroyParticles;
    public GameObject collisionParticles;

    [Header("Audio")]
    public AudioClip collisionSound;
    public AudioClip deathSound;

    [Range(0f, 1f)] public float collisionVolume = 0.5f;
    [Range(0f, 1f)] public float deathVolume = 0.7f;

    [Header("Pitch Variation")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

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

        sr.flipX = player.position.x < transform.position.x;

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

    void SpawnCollisionParticles(Vector2 position)
    {
        if (collisionParticles != null)
        {
            GameObject particles = Instantiate(
                collisionParticles,
                position,
                Quaternion.identity
            );

            Destroy(particles, 2f);
        }
    }

    void DestroyWithParticles()
    {

        PlaySound(deathSound, deathVolume);

        if (destroyParticles != null)
        {
            GameObject particles = Instantiate(
                destroyParticles,
                transform.position,
                Quaternion.identity
            );

            Destroy(particles, 2f);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore other slimes
        if (collision.gameObject.CompareTag("Googene"))
        {
            Physics2D.IgnoreCollision(
                collision.collider,
                GetComponent<Collider2D>()
            );
            return;
        }

   
        PlaySound(collisionSound, collisionVolume);

        // Spawn collision particles
        if (collision.contactCount > 0)
        {
            SpawnCollisionParticles(collision.contacts[0].point);
        }

        // Destroy if hitting a hazard
        if (collision.gameObject.CompareTag("Hazard"))
        {
            DestroyWithParticles();
            return;
        }

        // Ground check
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }

        // Player interaction
        if (collision.gameObject.CompareTag("Player"))
        {
            bool stomped = false;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    stomped = true;
                    break;
                }
            }

            if (stomped)
            {
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 6f);
                }

                DestroyWithParticles();
            }
            else
            {
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

    void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = transform.position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.pitch = Random.Range(minPitch, maxPitch);

        aSource.Play();

        Destroy(tempGO, clip.length / Mathf.Abs(aSource.pitch));
    }
}