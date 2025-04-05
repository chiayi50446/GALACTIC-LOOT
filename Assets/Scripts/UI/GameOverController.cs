using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TMP_Text gameOverTitle;
    [SerializeField] private GameObject starList;
    [SerializeField] private GameObject starUsedTime;
    [SerializeField] private GameObject starCollect;
    [SerializeField] private GameObject starSurvive;
    [SerializeField] private TMP_Text timerTargetText;
    [SerializeField] private TMP_Text timerText;
    private Level currentLevel;

    void Start()
    {
        Time.timeScale = 1;
        menuButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.playButtonSound();
            GameState.Instance.SetCurrentLevel(Level.Level1);
            SceneManager.LoadScene("StartScene");
        });
        restartButton.onClick.AddListener(Restart);
        if (GameState.Instance.GetCurrentLevel() == Level.Level2)
            nextLevelButton.gameObject.SetActive(false);
        nextLevelButton.onClick.AddListener(NextLevel);

        currentLevel = GameState.Instance.GetCurrentLevel();
        bool isClear = GameState.Instance.GetIsLevelClear(currentLevel);
        if (isClear)
        {
            gameOverTitle.text = "Mission Completed";
            starUsedTime.SetActive(false);
            starCollect.SetActive(false);
            starSurvive.SetActive(false);
            starList.SetActive(true);

            float usedTime = GameState.Instance.GetClearLevelTime(currentLevel);
            float targetTime = GameState.ClearTargetTime[currentLevel];
            if (usedTime <= targetTime)
            {
                starUsedTime.SetActive(true);
            }
            int usedMinutes = Mathf.FloorToInt(usedTime / 60);
            int usedSeconds = Mathf.FloorToInt(usedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", usedMinutes, usedSeconds);

            int targetMinutes = Mathf.FloorToInt(targetTime / 60);
            int targetSeconds = Mathf.FloorToInt(targetTime % 60);
            timerTargetText.text = string.Format("{0:00}:{1:00}", targetMinutes, targetSeconds);

            if (GameState.Instance.GetIsCollectItemGet(currentLevel))
            {
                starCollect.SetActive(true);
            }
            if (GameState.Instance.GetPlayerDeathNum(currentLevel) == 0)
            {
                starSurvive.SetActive(true);
            }
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
    void Update()
    {
        MoveSelection();
    }

    private void Restart()
    {
        // Time.timeScale = 1;
        AudioManager.Instance.playButtonSound();
        GameState.Instance.ResetCurrentLevel(currentLevel);
        DataPersistentManager.instance.EntryGame();
    }

    private void NextLevel()
    {
        // Time.timeScale = 1;
        AudioManager.Instance.playButtonSound();
        GameState.Instance.SetCurrentLevel(Level.Level2);
        GameState.Instance.ResetCurrentLevel(Level.Level2);
        SceneManager.LoadScene("StartScene");
    }

    private void MoveSelection()
    {
        Selectable current = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();
        if (current != null)
        {
            Selectable next = null;
            if (Input.GetKeyDown(KeyCode.L)) next = current.FindSelectableOnRight();
            if (Input.GetKeyDown(KeyCode.J)) next = current.FindSelectableOnLeft();
            if (Input.GetKeyDown(KeyCode.I)) next = current.FindSelectableOnUp();
            if (Input.GetKeyDown(KeyCode.K)) next = current.FindSelectableOnDown();

            if (next != null)
            {
                EventSystem.current.SetSelectedGameObject(next.gameObject);
            }
        }
    }
}
