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
    private bool isWalk;
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
    }

    void Update()
    {
        if (state == BossStates.Chasing && currentTarget != null)
        {
            if (!isWalk)
                anim.SetBool("isWalk", true);
            destination = (currentTarget.transform.position - transform.position).normalized;
            Vector3 temp = transform.position + destination * speed * Time.deltaTime;
            if (temp.x > 64f) temp.x = 64f;
            if (temp.x < 43.5f) temp.x = 43.5f;
            if (temp.y > 61f) temp.y = 61f;
            if (temp.y < 50.5f) temp.y = 50.5f;
            transform.position = temp;
        }
    }

    private void StartAttack()
    {
        if (playerList.Count == 0) return;

        currentTarget = selectTarget();
        state = BossStates.Attacking;
        anim.Play("Attack");
        Invoke(nameof(EnableAttackCollider), 0.4f);
        Debug.Log($"Boss attacks {currentTarget.name}");

        Invoke(nameof(ResetState), 1.5f);
    }

    private void ResetState()
    {
        state = BossStates.Chasing;
    }

    private GameObject selectTarget()
    {
        int choice = Random.Range(0, playerList.Count);
        return playerList[choice];
    }

    private void ActiveBoss()
    {
        InvokeRepeating(nameof(StartAttack), 2f, 5f); // 每 5 秒選擇目標攻擊
    }


    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
        Invoke(nameof(DisableAttackCollider), 0.5f); // 0.5 秒後自動關閉
    }

    void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }
}
