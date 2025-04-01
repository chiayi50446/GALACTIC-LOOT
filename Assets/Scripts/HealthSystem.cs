using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    private int health = 1;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Level currentLevel;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLevel = GameState.Instance.GetCurrentLevel();
        if (spriteRenderer == null)
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        originalColor = spriteRenderer.color; // Store original color
        if (CompareTag("Guard"))
        {
            health = 2;
        }
    }

    public void SetHealth(int h)
    {
        health = h;
    }
    public int GetHealth()
    {
        return health;
    }
    public void TakeDamege(int attackPoint)
    {
        AudioManager.Instance.playHurtSound();
        StartCoroutine(FlashRed());
        health -= attackPoint;
        if (healthBar != null)
            healthBar.GetComponent<BarController>().SetValue(health);
        if (health <= 0)
        {
            if (CompareTag("Boss"))
            {
                GameState.Instance.SetIsLevelClear(currentLevel, true);
                EventManager.Instance.TriggerBossDead();
            }
            else
            {
                Destroy(gameObject);
            }
            if (CompareTag("Player"))
            {
                GameState.Instance.SetPlayerDeathNum(currentLevel);
                if (GameState.Instance.GetPlayerDeathNum(currentLevel) == 2)
                {
                    DataPersistentManager.instance.EndGame();
                }
            }
        }
    }
    private System.Collections.IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red; // Change to red
        yield return new WaitForSeconds(0.1f); // Wait
        spriteRenderer.color = originalColor; // Restore original color
    }
}
