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

        [Header("Audio")]
        public AudioClip attackSound;
        [Range(0f, 1f)] public float volume = 0.7f;
    }

    [Header("Animators")]
    public Animator defenderAnimator;
    public Animator monsterAnimator;

    [Header("Sprite Renderers")]
    public SpriteRenderer defenderRenderer;
    public SpriteRenderer monsterRenderer;

    [Header("Sorting Settings")]
    public int baseSortingOrder = 0;
    public int attackSortingBoost = 5;

    [Header("Animation Triggers")]
    public string hurtTrigger = "Hurt";

    [Header("Defender Attacks")]
    public Attack[] defenderAttacks;

    [Header("GooBeast Attacks")]
    public Attack[] gooBeastAttacks;

    [Header("Battle Timing")]
    public float timeBetweenAttacks = 1f;

    [Header("Difficulty Scaling")]
    public float minTimeBetweenAttacks = 0.2f;
    public float speedIncreaseRate = 0.05f;

    [Header("Pitch Variation")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private bool defenderTurn = true;

    void Start()
    {
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

            timeBetweenAttacks = Mathf.Max(
                minTimeBetweenAttacks,
                timeBetweenAttacks - speedIncreaseRate
            );

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    IEnumerator DoAttack(Animator anim, Attack[] attacks, SpriteRenderer attacker, SpriteRenderer defender)
    {
        if (attacks.Length == 0)
            yield break;

        Attack chosen = attacks[Random.Range(0, attacks.Length)];

        attacker.sortingOrder = baseSortingOrder + attackSortingBoost;
        defender.sortingOrder = baseSortingOrder;


        PlaySound(chosen.attackSound, chosen.volume, attacker.transform.position);

        if (anim != null && chosen.animationTrigger != "")
            anim.SetTrigger(chosen.animationTrigger);

        yield return new WaitForSeconds(chosen.spawnDelay);

        if (chosen.attackPattern != null)
            chosen.attackPattern.SendMessage("SpawnWave");

        // Hurt animation
        Animator defenderAnim = defender.GetComponent<Animator>();
        if (defenderAnim != null && hurtTrigger != "")
        {
            defenderAnim.SetTrigger(hurtTrigger);
        }

        float remainingTime = chosen.attackDuration - chosen.spawnDelay;

        if (remainingTime > 0)
            yield return new WaitForSeconds(remainingTime);

        attacker.sortingOrder = baseSortingOrder;
        defender.sortingOrder = baseSortingOrder;
    }


    void PlaySound(AudioClip clip, float volume, Vector3 position)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAttackAudio");
        tempGO.transform.position = position;

        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(minPitch, maxPitch);

        source.Play();

        Destroy(tempGO, clip.length / Mathf.Abs(source.pitch));
    }
}