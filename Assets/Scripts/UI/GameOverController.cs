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
    [SerializeField] private GameObject starUsedTime;
    [SerializeField] private GameObject starCollect;
    [SerializeField] private GameObject starSurvive;
    [SerializeField] private TMP_Text timerTargetText;
    [SerializeField] private TMP_Text timerText;
    private Level currentLevel;

    void Start()
    {
        menuButton.onClick.AddListener(() => SceneManager.LoadScene("StartScene"));
        restartButton.onClick.AddListener(Restart);

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
            float targetTime = GameState.clearTargetTime[currentLevel];
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

    private void Restart()
    {
        // int currentLevel = GameState.Instance.GetCurrentLevel();
        // GameState.Instance.CreateNewGame();
        // GameState.Instance.SetCurrentLevel(currentLevel);
        DataPersistentManager.instance.EntryGame();
    }
}
