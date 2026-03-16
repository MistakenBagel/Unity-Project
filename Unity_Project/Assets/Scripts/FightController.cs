using System.Collections;
using UnityEngine;

public class FightController : MonoBehaviour
{
    [System.Serializable]
    public class Attack
    {
        public string animationTrigger;      // Animator trigger name
        public MonoBehaviour attackPattern;  // Script with SpawnWave()

        public float spawnDelay = 0.4f;      // Delay before debris spawns
        public float attackDuration = 2f;    // Total attack time
    }

    [Header("Animators")]
    public Animator defenderAnimator;
    public Animator gooBeastAnimator;

    [Header("Defender Attacks")]
    public Attack[] defenderAttacks;

    [Header("GooBeast Attacks")]
    public Attack[] gooBeastAttacks;

    [Header("Battle Timing")]
    public float timeBetweenAttacks = 1f;

    private bool defenderTurn = true;

    void Start()
    {
        StartCoroutine(FightLoop());
    }

    IEnumerator FightLoop()
    {
        // Small delay before battle starts
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (defenderTurn)
                yield return StartCoroutine(DoAttack(defenderAnimator, defenderAttacks));
            else
                yield return StartCoroutine(DoAttack(gooBeastAnimator, gooBeastAttacks));

            defenderTurn = !defenderTurn;

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    IEnumerator DoAttack(Animator anim, Attack[] attacks)
    {
        if (attacks.Length == 0)
            yield break;

        Attack chosen = attacks[Random.Range(0, attacks.Length)];

        // Play animation
        if (anim != null && chosen.animationTrigger != "")
            anim.SetTrigger(chosen.animationTrigger);

        // Wait until impact moment
        yield return new WaitForSeconds(chosen.spawnDelay);

        // Trigger debris or other attack pattern
        if (chosen.attackPattern != null)
            chosen.attackPattern.SendMessage("SpawnWave");

        // Wait remaining attack time
        float remainingTime = chosen.attackDuration - chosen.spawnDelay;

        if (remainingTime > 0)
            yield return new WaitForSeconds(remainingTime);
    }
}