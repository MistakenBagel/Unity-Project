using UnityEngine;
using System.Collections;

public class GooPuddle : MonoBehaviour
{
    [Header("Lifetime")]
    public float puddleDuration = 5f;

    [Header("Player Effects")]
    public float slowMultiplier = 0.5f;
    public float jumpDisableTime = 2f;

    private Collider2D puddleCollider;

    void Start()
    {
        puddleCollider = GetComponent<Collider2D>();

        Destroy(gameObject, puddleDuration);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D playerCollider = collision.collider;

            // Let player pass through the puddle
            Physics2D.IgnoreCollision(playerCollider, puddleCollider);

            StartCoroutine(ApplyEffect(collision.gameObject));
        }
    }

    IEnumerator ApplyEffect(GameObject player)
    {
        PlayerController2D movement = player.GetComponent<PlayerController2D>();

        if (movement != null)
        {
            float originalSpeed = movement.moveSpeed;

            movement.moveSpeed *= slowMultiplier;
            movement.canJump = false;

            yield return new WaitForSeconds(jumpDisableTime);

            movement.moveSpeed = originalSpeed;
            movement.canJump = true;
        }
    }
}