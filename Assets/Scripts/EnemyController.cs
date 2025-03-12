using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float newAlertnessLevel = GameState.Instance.GetAlertnessLevel();
            GameState.Instance.SetAlertnessLevel(newAlertnessLevel + 1);
            EventManager.Instance.TriggerUpdateAlertnessLevel();
        }
    }
}
