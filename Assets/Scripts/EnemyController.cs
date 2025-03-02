using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private string[] arrDirection = { "left", "down", "right" };
    [SerializeField] private int nowDirection = 0;
    void Start()
    {
        InvokeRepeating("Rotate", 0f, 5f); // Rotate every five seconds
    }

    void Update()
    {

    }
    void Rotate()
    {
        nowDirection = (nowDirection + 1) % 3;
        EnemyVisionController visionController = GetComponentInChildren<EnemyVisionController>();
        visionController.SetDirection(arrDirection[nowDirection]);
    }
}
