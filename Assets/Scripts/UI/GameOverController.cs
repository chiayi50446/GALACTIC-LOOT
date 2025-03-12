using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TMP_Text gameOverTitle;
    [SerializeField] private GameObject starList;
    private bool isClear;

    void Start()
    {
        menuButton.onClick.AddListener(() => SceneManager.LoadScene("StartScene"));
        restartButton.onClick.AddListener(Restart);
        isClear = GameState.Instance.GetIsLevelClear();

        if (isClear)
        {
            gameOverTitle.text = "Mission Completed";
            starList.SetActive(true);
        }
        else
        {
            gameOverTitle.text = "Mission Failure";
            starList.SetActive(false);
            nextLevelButton.interactable = false;
        }
    }
    void OnEnable()
    {
        if (menuButton != null)
        {
            menuButton.GetComponent<Button>().Select();
        }
    }

    private void Restart()
    {
        // int currentLevel = GameState.Instance.GetCurrentLevel();
        // GameState.Instance.CreateNewGame();
        // GameState.Instance.SetCurrentLevel(currentLevel);
        DataPersistentManager.instance.EntryGame();
    }
}
