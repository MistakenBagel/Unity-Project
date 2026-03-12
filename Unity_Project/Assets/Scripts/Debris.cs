using UnityEngine;

public class Debris : MonoBehaviour
{
    private Animator anim;

    public GameObject destroyParticles;
    public GameObject shootPrefab;

    public float shootForce = 6f;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Prevent hazards from colliding with other hazards
        IgnoreHazardCollisions();

        // Start animation at random frame
        if (anim != null)
        {
            float randomStart = Random.Range(0f, 1f);
            anim.Play(0, -1, randomStart);
            anim.speed = Random.Range(0.8f, 1.2f);
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            SpawnParticles();
            ShootObjects();
            Destroy(gameObject);
        }
    }

    void SpawnParticles()
    {
        if (destroyParticles != null)
        {
            GameObject particles = Instantiate(
                destroyParticles,
                transform.position,
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
}