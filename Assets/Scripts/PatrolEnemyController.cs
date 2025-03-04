using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyController : MonoBehaviour
{
    private string[] arrDirection = { "left", "down", "right", "up" };
    [SerializeField] private int nowDirection = 0;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float distanceThreshold = 0.5f;
    [SerializeField] private float moveSpeed = 1f;
    private Animator anim;
    private bool isLeft;
    private int index = 0;
    private Vector3 destination;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("direction", nowDirection);
        isLeft = (nowDirection != 2) ? true : false;
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
            nowDirection = 2;
        }
        else if (destination.x - transform.position.x < 0)
        {
            nowDirection = 0;
        }
        else if (destination.y - transform.position.y < 0)
        {
            nowDirection = 1;
        }
        else if (destination.y - transform.position.y < 0)
        {
            nowDirection = 3;
        }
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

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
