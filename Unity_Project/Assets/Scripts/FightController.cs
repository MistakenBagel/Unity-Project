using System.Collections;
using UnityEngine;

public class FightController : MonoBehaviour
{
    [System.Serializable]
    public class Attack
    {
        public string animationTrigger;
        public MonoBehaviour attackPattern;

        public float spawnDelay = 0.4f;
        public float attackDuration = 2f;
    }

    [Header("Animators")]
    public Animator defenderAnimator;
    public Animator monsterAnimator;

    [Header("Sprite Renderers")]
    public SpriteRenderer defenderRenderer;
    public SpriteRenderer monsterRenderer;

    [Header("Sorting Settings")]
    public int baseSortingOrder = 0;
    public int attackSortingBoost = 5; // how much higher attacker goes

    [Header("Defender Attacks")]
    public Attack[] defenderAttacks;

    [Header("GooBeast Attacks")]
    public Attack[] gooBeastAttacks;

    [Header("Battle Timing")]
    public float timeBetweenAttacks = 1f;

    private bool defenderTurn = true;

    void Start()
    {
        // Ensure both start at same layer
        defenderRenderer.sortingOrder = baseSortingOrder;
        monsterRenderer.sortingOrder = baseSortingOrder;

        StartCoroutine(FightLoop());
    }

    IEnumerator FightLoop()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (defenderTurn)
                yield return StartCoroutine(DoAttack(defenderAnimator, defenderAttacks, defenderRenderer, monsterRenderer));
            else
                yield return StartCoroutine(DoAttack(monsterAnimator, gooBeastAttacks, monsterRenderer, defenderRenderer));

            defenderTurn = !defenderTurn;

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    IEnumerator DoAttack(Animator anim, Attack[] attacks, SpriteRenderer attacker, SpriteRenderer defender)
    {
        if (attacks.Length == 0)
            yield break;

        Attack chosen = attacks[Random.Range(0, attacks.Length)];

        // Bring attacker to front
        attacker.sortingOrder = baseSortingOrder + attackSortingBoost;
        defender.sortingOrder = baseSortingOrder;

        // Play animation
        if (anim != null && chosen.animationTrigger != "")
            anim.SetTrigger(chosen.animationTrigger);

        // Wait until impact moment
        yield return new WaitForSeconds(chosen.spawnDelay);

        if (chosen.attackPattern != null)
            chosen.attackPattern.SendMessage("SpawnWave");

        float remainingTime = chosen.attackDuration - chosen.spawnDelay;

        if (remainingTime > 0)
            yield return new WaitForSeconds(remainingTime);

        // Reset both back to normal
        attacker.sortingOrder = baseSortingOrder;
        defender.sortingOrder = baseSortingOrder;
    }
}