using UnityEngine;

public class ExplosionBombController : MonoBehaviour
{
    public float explosionRadius = 1.5f;
    public float explosionDelay = 1.2f;
    public int explosionDamage = 3;
    public LayerMask enemyLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void OnEnable()
    {
        Invoke("Explode", explosionDelay);
    }

    void Explode()
    {
        // 在 explosionRadius 內找到所有敵人
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

    }

    // 繪製爆炸範圍（僅在開發時可見）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
