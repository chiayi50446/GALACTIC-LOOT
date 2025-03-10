using System.Collections.Generic;
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
    private Animator anim;
    private bool isStartAttack;
    private Vector3 destination;
    private GameObject currentTarget;

    void Start()
    {
        anim = GetComponent<Animator>();
        state = BossStates.Idle;
        InvokeRepeating(nameof(StartAttack), 2f, 5f); // 每 5 秒選擇目標攻擊
    }

    void Update()
    {
        // switch (state)
        // {
        //     case BossStates.Idle:
        //         break;
        //     case BossStates.Chasing:
        //         destination = selectTarget().transform.position;
        //         break;
        //     default:
        //         Debug.LogError("State not configured", this);
        //         break;
        // }
        if (state == BossStates.Chasing && currentTarget != null)
        {
            destination = currentTarget.transform.position;
            // 這裡可以加上移動的邏輯，讓 Boss 追蹤目標
        }
    }

    private void FixedUpdate()
    {
        if (!isStartAttack)
        {
            state = BossStates.Idle;
        }
        else
        {
            state = BossStates.Chasing;
        }
    }

    private void StartAttack()
    {
        if (playerList.Count == 0) return;

        currentTarget = selectTarget();
        state = BossStates.Attacking;
        anim.Play("Attack"); // 觸發攻擊動畫
        Debug.Log($"Boss attacks {currentTarget.name}");

        // 等待一段時間後回到 Idle 或 Chasing
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
}
