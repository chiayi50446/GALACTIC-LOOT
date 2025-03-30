using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossStates
    {
        Idle,
        Chasing,
        Attacking,
        Death
    };

    [SerializeField] private BossStates state;
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private GameObject dessert;
    [SerializeField] private Animator anim;
    [SerializeField] private float distanceThreshold = 5f;
    [SerializeField] private float attackRate = 2f;
    private bool isWalk;
    private bool isLeft;
    private Vector3 destination;
    private GameObject currentTarget;
    private float speed = 5f;
    private float nextAttackTime;

    void OnEnable()
    {
        EventManager.Instance.ActiveBoss += ActiveBoss;
        EventManager.Instance.BossDead += BossDead;
    }

    void OnDisable()
    {
        EventManager.Instance.ActiveBoss -= ActiveBoss;
        EventManager.Instance.BossDead -= BossDead;
    }

    void Start()
    {
        state = BossStates.Idle;
        attackCollider.enabled = false;
        isLeft = true;
        nextAttackTime = 0f;
        GetComponent<HealthSystem>().SetHealth(GameState.Instance.BossHealth[GameState.Instance.GetCurrentLevel()]);
    }

    void Update()
    {
        if (state == BossStates.Chasing && currentTarget != null)
        {
            if (!isWalk) anim.SetBool("isWalk", true);
            if (Vector3.Distance(currentTarget.transform.position, transform.position) < distanceThreshold)
            {
                if (Time.time > nextAttackTime)
                {
                    StartAttack();
                }
            }
            else
            {
                if (currentTarget.transform.position.x > transform.position.x)
                {
                    if (isLeft)
                    {
                        Flip();
                        isLeft = false;
                    }
                }
                else
                {
                    if (!isLeft)
                    {
                        Flip();
                        isLeft = true;
                    }
                }
                destination = (currentTarget.transform.position - transform.position).normalized;
                Vector3 temp = transform.position + destination * speed * Time.deltaTime;
                if (temp.x > 70f) temp.x = 70f;
                if (temp.x < 43.5f) temp.x = 43.5f;
                if (temp.y > 61f) temp.y = 61f;
                if (temp.y < 45f) temp.y = 45f;
                transform.position = temp;
            }
        }
    }

    private void StartAttack()
    {
        nextAttackTime = Time.time + attackRate;
        state = BossStates.Attacking;
        anim.Play("Attack");
        anim.SetBool("isWalk", false);
        isWalk = false;
        Invoke(nameof(EnableAttackCollider), 0.4f);
        Debug.Log($"Boss attacks {currentTarget.name}");

        Invoke(nameof(ResetState), 0.9f);
    }

    private void ResetState()
    {
        selectTarget();
        state = BossStates.Chasing;
    }

    private void selectTarget()
    {
        if (playerList.Count == 0) return;

        int randomNum = Random.Range(0, playerList.Count);
        GameObject newTarget = playerList[randomNum];
        if ((GameState.Instance.GetPlayerDeathNum(GameState.Instance.GetCurrentLevel()) == 0 && newTarget == currentTarget)
         || newTarget == null)
        {
            newTarget = playerList[(randomNum + 1) % playerList.Count];
        }
        currentTarget = newTarget;
    }

    private void ActiveBoss()
    {
        state = BossStates.Chasing;
        selectTarget();
    }

    private void BossDead()
    {
        state = BossStates.Death;
        anim.Play("Death");
        StartCoroutine(Helper.Delay_RealTime(() =>
        {
            Time.timeScale = 0;
            transform.GetChild(0).gameObject.SetActive(false);
            dessert.SetActive(true);
            StartCoroutine(Helper.Delay_RealTime(() =>
            {
                EventManager.Instance.TriggerClearLevel();
                DataPersistentManager.instance.EndGame();
            }, 1.5f));
        }, 0.85f));
    }


    public void EnableAttackCollider()
    {
        if (attackCollider == null) return;
        attackCollider.enabled = true;
        Invoke(nameof(DisableAttackCollider), 0.5f);
    }

    void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
