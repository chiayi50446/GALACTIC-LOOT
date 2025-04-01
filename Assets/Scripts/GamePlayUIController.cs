using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    private BarController alertnessBar;
    private Level currentLevel;
    private bool isStopRecordTime;
    private float usedTime;
    private bool isPauseMenuOpen = false;

    void Awake()
    {
        EventManager.Instance.UpdateAlertnessLevel += UpdateAlertnessLevel;
        EventManager.Instance.ActiveBossHealth += OpenBossRoomUI;
        EventManager.Instance.ClearLevel += RecordClearTime;
    }

    void OnDestroy()
    {
        EventManager.Instance.UpdateAlertnessLevel -= UpdateAlertnessLevel;
        EventManager.Instance.ActiveBossHealth -= OpenBossRoomUI;
        EventManager.Instance.ClearLevel -= RecordClearTime;
    }

    void Start()
    {
        Time.timeScale = 1;
        alertnessBar = GetComponentInChildren<BarController>();
        alertnessBar.SetValue(0);
        GameState.Instance.SetAlertnessLevel(0);
        EventManager.Instance.TriggerUpdateVision();
        currentLevel = GameState.Instance.GetCurrentLevel();
        usedTime = 0;
        isStopRecordTime = false;
        menuButton.onClick.AddListener(BackToMenu);
        // restartButton.onClick.AddListener(Restart);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            isPauseMenuOpen = !isPauseMenuOpen;
            PauseMenu.SetActive(isPauseMenuOpen);
        }
    }

    void FixedUpdate()
    {
        UpdateTimer();
    }

    private void UpdateAlertnessLevel()
    {
        AudioManager.Instance.playEnemyVisionSound();
        float nowAlertnessLevel = GameState.Instance.GetAlertnessLevel();
        alertnessBar.SetValue(nowAlertnessLevel);
        EventManager.Instance.TriggerUpdateVision();
        if (nowAlertnessLevel >= 3)
        {
            EventManager.Instance.TriggerActiveNegotiationCheck();
        }
    }

    void OpenBossRoomUI()
    {
        gameObject.transform.GetChild(0).Find("AlertnessLevel").gameObject.SetActive(false);
        gameObject.transform.GetChild(0).Find("BossHealth").gameObject.SetActive(true);
    }

    private void UpdateTimer()
    {
        if (!isStopRecordTime)
        {
            usedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(usedTime / 60);
            int seconds = Mathf.FloorToInt(usedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void RecordClearTime()
    {
        GameState.Instance.SetClearLevelTime(currentLevel, usedTime);
    }
    public void Restart()
    {
        Debug.Log("Restart");
        AudioManager.Instance.playButtonSound();
        GameState.Instance.ResetCurrentLevel(currentLevel);
        DataPersistentManager.instance.EntryGame();
    }
    private void BackToMenu()
    {
        Debug.Log("BackToMenu");
        AudioManager.Instance.playButtonSound();
        GameState.Instance.SetCurrentLevel(Level.Level1);
        SceneManager.LoadScene("StartScene");
    }
}
