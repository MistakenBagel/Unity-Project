using System.Collections;
using UnityEngine;

public class FightController : MonoBehaviour
{
    [System.Serializable]
    public class Attack
    {
        public string animationTrigger;
        public MonoBehaviour attackPattern;
        public float attackDuration = 2f;
    }

    public Animator defenderAnimator;
    public Animator gooBeastAnimator;

    public Attack[] defenderAttacks;
    public Attack[] gooBeastAttacks;

    public float timeBetweenAttacks = 1f;

    private bool defenderTurn = true;

    void Start()
    {
        StartCoroutine(FightLoop());
    }

    IEnumerator FightLoop()
    {
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

        if (anim != null && chosen.animationTrigger != "")
            anim.SetTrigger(chosen.animationTrigger);

        if (chosen.attackPattern != null)
        {
            chosen.attackPattern.SendMessage("SpawnWave");
        }

        yield return new WaitForSeconds(chosen.attackDuration);
    }
}