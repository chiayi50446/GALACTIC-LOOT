using UnityEngine;

public class PunchController : MonoBehaviour
{
    public bool isUsingGun = false;
    public int damage;
    public LayerMask enemyLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUsingGun)
        {
            if (enemyLayer == (enemyLayer | (1 << other.gameObject.layer))) // 如果碰到敵人
            {
                var healthSystem = other.GetComponent<HealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.TakeDamege(damage);
                }
            }
        }
    }
}
