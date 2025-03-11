using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // 子彈速度
    public float lifeTime = 0.5f; // 子彈存活時間
    public int damage = 2;
    public LayerMask enemyLayer;


    void Start()
    {
        Destroy(gameObject, lifeTime); // 過 `lifeTime` 秒後銷毀子彈
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyLayer == (enemyLayer | (1 << other.gameObject.layer))) // 如果碰到敵人
        {
            var healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamege(damage);
            }
            Destroy(gameObject); // 銷毀子彈
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyLayer != (enemyLayer | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
