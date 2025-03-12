using UnityEngine;

public class StationaryEnemyController : EnemyController
{
    private string[] arrDirection = { "left", "down", "right", "up" };
    [SerializeField] private int nowDirection = 0;
    private Animator anim;
    private bool isLeft;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("direction", nowDirection);
        InvokeRepeating("Rotate", 0f, 5f); // Rotate every five seconds
        isLeft = true;
    }

    void Update()
    {

    }
    void Rotate()
    {
        nowDirection = (nowDirection + 1) % 3;
        anim.SetInteger("direction", nowDirection);
        if (arrDirection[nowDirection] == "left")
        {
            if (!isLeft)
            {
                isLeft = true;
                Flip();
            }
        }
        if (arrDirection[nowDirection] == "right")
        {
            if (isLeft)
            {
                isLeft = false;
                Flip();
            }
        }
        EnemyVisionController visionController = GetComponentInChildren<EnemyVisionController>();
        visionController.SetDirection(arrDirection[nowDirection]);
    }
}
