using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossStates
    {
        Idle,
        Chasing,
        Attacking
    };

    [SerializeField] private BossStates state;
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private Animator anim;
    [SerializeField] private float distanceThreshold = 5f;
    private bool isWalk;
    private bool isLeft;
    private Vector3 destination;
    private GameObject currentTarget;
    private float speed = 5f;

    void OnEnable()
    {
        EventManager.Instance.ActiveBoss += ActiveBoss;
    }

    void OnDisable()
    {
        EventManager.Instance.ActiveBoss -= ActiveBoss;
    }

    void Start()
    {
        state = BossStates.Idle;
        attackCollider.enabled = false;
        isLeft = true;
    }

    void Update()
    {
        if (state == BossStates.Chasing && currentTarget != null)
        {
            if (!isWalk) anim.SetBool("isWalk", true);
            if (Vector3.Distance(currentTarget.transform.position, transform.position) < distanceThreshold)
            {
                StartAttack();
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
                if (temp.x > 64f) temp.x = 64f;
                if (temp.x < 43.5f) temp.x = 43.5f;
                if (temp.y > 61f) temp.y = 61f;
                if (temp.y < 50.5f) temp.y = 50.5f;
                transform.position = temp;
            }
        }
    }

    private void RandomTarget()
    {
        if (playerList.Count == 0) return;
        currentTarget = selectTarget();
    }

    private void StartAttack()
    {
        state = BossStates.Attacking;
        anim.Play("Attack");
        anim.SetBool("isWalk", false);
        isWalk = false;
        Invoke(nameof(EnableAttackCollider), 0.4f);
        Debug.Log($"Boss attacks {currentTarget.name}");

        Invoke(nameof(ResetState), 1.5f);
    }

    private void ResetState()
    {
        state = BossStates.Chasing;
        selectTarget(true);
    }

    private GameObject selectTarget(bool notCurrent = false)
    {
        if (playerList.Count == 0) return null;

        if (notCurrent)
        {
            GameObject newTarget;
            do
            {
                newTarget = playerList[Random.Range(0, playerList.Count)];
            } while (newTarget == currentTarget);
            return newTarget;
        }

        return playerList[Random.Range(0, playerList.Count)];
    }

    private void ActiveBoss()
    {
        state = BossStates.Chasing;
        InvokeRepeating(nameof(RandomTarget), 2f, 5f);
    }


    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
        Invoke(nameof(DisableAttackCollider), 0.5f);
    }

    void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
