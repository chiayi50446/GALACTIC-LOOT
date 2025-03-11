using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;

    void Start()
    {
        menuButton.onClick.AddListener(() => SceneManager.LoadScene("StartScene"));
        restartButton.onClick.AddListener(() => DataPersistentManager.instance.EntryGame());
        nextLevelButton.interactable = false;
    }
    void OnEnable()
    {
        if (menuButton != null)
        {
            menuButton.GetComponent<Button>().Select();
        }
    }
}
