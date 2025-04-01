using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    private bool isPauseMenuOpen = false;
    private Level currentLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = GameState.Instance.GetCurrentLevel();
        menuButton.onClick.AddListener(BackToMenu);
        restartButton.onClick.AddListener(Restart);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPauseMenuOpen = !isPauseMenuOpen;
            Time.timeScale = isPauseMenuOpen ? 0 : 1;
            PauseMenu.SetActive(isPauseMenuOpen);
        }
    }
    public void Restart()
    {
        AudioManager.Instance.playButtonSound();
        GameState.Instance.ResetCurrentLevel(currentLevel);
        DataPersistentManager.instance.EntryGame();
    }
    private void BackToMenu()
    {
        AudioManager.Instance.playButtonSound();
        GameState.Instance.SetCurrentLevel(Level.Level1);
        SceneManager.LoadScene("StartScene");
    }
}
