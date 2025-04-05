using UnityEngine;


public class StationaryEnemyController : EnemyController
{
    private string[] arrDirection = { "left", "down", "right", "up" };
    [SerializeField] private Direction nowDirection;
    [SerializeField] private Direction[] moveDirection;
    private Animator anim;
    private bool isLeft;

    void Start()
    {
        type = EnemyType.stationary;
        anim = GetComponent<Animator>();
        anim.SetInteger("direction", (int)nowDirection);
        InvokeRepeating("Rotate", 0f, 5f); // Rotate every five seconds
        isLeft = true;
    }

    void OnDestroy()
    {
        CancelInvoke("Rotate");
    }

    void NextDirection()
    {
        if (moveDirection.Length == 0) return;
        int index = System.Array.IndexOf(moveDirection, nowDirection);
        nowDirection = moveDirection[(index + 1) % moveDirection.Length];
    }
    void Rotate()
    {
        NextDirection();
        anim.SetInteger("direction", (int)nowDirection);
        if (nowDirection == Direction.right)
        {
            if (isLeft)
            {
                isLeft = false;
                Flip();
            }
        }
        else
        {
            if (!isLeft)
            {
                isLeft = true;
                Flip();
            }
        }
        EnemyVisionController visionController = GetComponentInChildren<EnemyVisionController>();
        visionController.SetDirection(nowDirection);
    }
}
