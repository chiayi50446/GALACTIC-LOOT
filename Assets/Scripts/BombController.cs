using UnityEngine;

public class BombController : MonoBehaviour
{
    public Vector2 startPoint;
    public Vector2 controlPoint;
    public Vector2 targetPoint;
    public float duration = 1.5f;
    public bool IsThrowing = false;

    private float timeElapsed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsThrowing)
        {
            if (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / duration;

                // 使用二次貝塞爾曲線公式
                transform.position = Mathf.Pow(1 - t, 2) * startPoint +
                                     2 * (1 - t) * t * controlPoint +
                                     Mathf.Pow(t, 2) * targetPoint;
            }
        }
        else
        {
            timeElapsed = 0;
        }
    }
}
