using UnityEngine;

public class Debris : MonoBehaviour
{
    private Animator anim;

    public GameObject destroyParticles;
    public GameObject shootPrefab;

    [Header("Optional Replacement")]
    public GameObject replaceOnGroundPrefab;

    public float shootForce = 6f;

    [Header("Optional Ground Lifetime")]
    public float stayOnGroundTime = 0f;

    [Header("Bounds Check")]
    public float minX = -1f;
    public float maxX = 12f;

    [Header("Audio")]
    public AudioClip destroySound;

    private bool hasLanded = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        IgnoreHazardCollisions();

        if (anim != null)
        {
            float randomStart = Random.Range(0f, 1f);
            anim.Play(0, -1, randomStart);
            anim.speed = Random.Range(0.8f, 1.2f);
        }
    }

    void Update()
    {
        float x = transform.position.x;
        if (x < minX || x > maxX)
        {
            PlayDestroySound();
            Destroy(gameObject);
        }
    }

    void IgnoreHazardCollisions()
    {
        if (!CompareTag("Hazard")) return;

        Collider2D myCollider = GetComponent<Collider2D>();
        GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");

        foreach (GameObject hazard in hazards)
        {
            if (hazard == gameObject) continue;

            Collider2D otherCollider = hazard.GetComponent<Collider2D>();

            if (otherCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, otherCollider);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Spawn particles on EVERY collision
        if (collision.contactCount > 0)
        {
            SpawnParticles(collision.contacts[0].point);
        }

        if (hasLanded) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;

            ShootObjects();
            ReplaceObject();

            PlayDestroySound();

            if (stayOnGroundTime <= 0f)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, stayOnGroundTime);
            }
        }
    }

    void SpawnParticles(Vector2 position)
    {
        if (destroyParticles != null)
        {
            GameObject particles = Instantiate(
                destroyParticles,
                position,
                Quaternion.identity
            );

            Destroy(particles, 2f);
        }
    }

    void ShootObjects()
    {
        if (shootPrefab == null) return;

        Vector2 leftDir = new Vector2(-1, 1).normalized;
        Vector2 rightDir = new Vector2(1, 1).normalized;

        GameObject left = Instantiate(shootPrefab, transform.position, Quaternion.identity);
        GameObject right = Instantiate(shootPrefab, transform.position, Quaternion.identity);

        Rigidbody2D leftRB = left.GetComponent<Rigidbody2D>();
        Rigidbody2D rightRB = right.GetComponent<Rigidbody2D>();

        if (leftRB != null)
            leftRB.AddForce(leftDir * shootForce, ForceMode2D.Impulse);

        if (rightRB != null)
            rightRB.AddForce(rightDir * shootForce, ForceMode2D.Impulse);
    }

    void ReplaceObject()
    {
        if (replaceOnGroundPrefab == null) return;

        Instantiate(
            replaceOnGroundPrefab,
            transform.position,
            Quaternion.identity
        );
    }

    void PlayDestroySound()
    {
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
    }
}