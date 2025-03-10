using UnityEngine;

public class BossWeaponController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Attack" + other.name);
    }
}
