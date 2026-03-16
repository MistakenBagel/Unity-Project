using UnityEngine;

public class GooGlob : MonoBehaviour
{
    private Animator anim;

    public GameObject destroyParticles;

    [Header("Puddle Settings")]
    public GameObject puddlePrefab;

    private bool hasLanded = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        IgnoreHazardCollisions();

        // Randomize animation start
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

            if (myCollider != null && otherCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, otherCollider);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasLanded) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;

            SpawnParticles();
            SpawnPuddle();

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

    void SpawnPuddle()
    {
        if (puddlePrefab != null)
        {
            Instantiate(
                puddlePrefab,
                new Vector3(transform.position.x, transform.position.y - 0.2f, 0),
                Quaternion.identity
            );
        }
    }
}