using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    private int health;
    void Start()
    {
        if (CompareTag("Guard"))
        {
            health = 2;
        }
        if (CompareTag("Boss"))
        {
            health = GameState.Instance.BossHealth[GameState.Instance.GetCurrentLevel() - 1];
        }
    }
    public void TakeDamege(int attackPoint)
    {
        health -= attackPoint;
        healthBar.GetComponent<BarController>().SetValue(health);
        if (health <= 0)
        {
            Destroy(gameObject);

            if (CompareTag("Boss"))
            {
                DataPersistentManager.instance.EndGame();
            }
        }
    }
}
