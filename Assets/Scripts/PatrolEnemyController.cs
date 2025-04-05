using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyController : EnemyController
{
    private string[] arrDirection = { "left", "down", "right", "up" };
    [SerializeField] private Direction nowDirection = Direction.left;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float distanceThreshold = 0.5f;
    [SerializeField] private float moveSpeed = 1f;
    private Animator anim;
    private bool isLeft = true;
    private int index = 0;
    private Vector3 destination;
    void Start()
    {
        type = EnemyType.patrol;
        anim = GetComponent<Animator>();
        anim.SetInteger("direction", (int)nowDirection);
        if (nowDirection == Direction.right)
        {
            isLeft = false;
            Flip();
        }
        destination = waypoints[index].position;
        Rotate();
    }

    void Update()
    {
        Vector3 direction = (destination - transform.position).normalized;
        transform.position += direction * Time.deltaTime * moveSpeed;
        if (Vector3.Distance(destination, transform.position) < distanceThreshold)
        {
            index = (index + 1) % waypoints.Count;
            destination = waypoints[index].position;
            Rotate();
        }
    }
    void Rotate()
    {
        if (destination.x - transform.position.x > 0)
        {
            nowDirection = Direction.right;
        }
        else if (destination.x - transform.position.x < 0)
        {
            nowDirection = Direction.left;
        }
        else if (destination.y - transform.position.y < 0)
        {
            nowDirection = Direction.down;
        }
        else if (destination.y - transform.position.y > 0)
        {
            nowDirection = Direction.up;
        }
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
