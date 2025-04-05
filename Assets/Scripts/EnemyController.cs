using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected EnemyType type = EnemyType.none;
    public void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        Transform healthBarCanvas = transform.Find("HealthBarCanvas");
        if (healthBarCanvas != null)
        {
            healthBarCanvas.localScale *= -1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int playerType = (collision.gameObject.name == "Player1") ? 1 : 2;
            GameState.Instance.SetLastAlertPlayerType(playerType);
            GameState.Instance.SetLastAlertEnemyType(type);

            float newAlertnessLevel = GameState.Instance.GetAlertnessLevel();
            GameState.Instance.SetAlertnessLevel(newAlertnessLevel + 1);
            EventManager.Instance.TriggerUpdateAlertnessLevel();
        }
    }
}

public enum Direction
{
    left = 0,
    down = 1,
    right = 2,
    up = 3
}
