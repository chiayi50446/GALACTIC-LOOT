using UnityEngine;

public class BossWeaponController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.TakeDamege(1);
        }
    }
}
