using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    private int health;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    public void TakeDamege(int attackPoint)
    {
        StartCoroutine(FlashRed());
        health -= attackPoint;
        healthBar.GetComponent<BarController>().SetValue(health);
        if (health <= 0)
        {
            Destroy(gameObject);

            if (CompareTag("Boss"))
            {
                GameState.Instance.SetIsLevelClear(true);
                DataPersistentManager.instance.EndGame();
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
