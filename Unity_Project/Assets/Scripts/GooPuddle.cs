using UnityEngine;

public class GooPuddle : MonoBehaviour
{
    [Header("Spawn Offset")]
    public float yOffset = 0f;

    [Header("Puddle Lifetime")]
    public float lifetime = 5f;

    [Header("Player Effects")]
    public float speedMultiplier = 0.5f;
    public float effectDuration = 1.5f;
    public bool disableJump = true;

    void Start()
    {
        // Apply Y offset on spawn
        transform.position += new Vector3(0f, yOffset, 0f);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerController2D player = collision.GetComponent<PlayerController2D>();

        if (player != null)
        {
            player.ApplyGooEffect(speedMultiplier, effectDuration, disableJump);
        }
    }
}