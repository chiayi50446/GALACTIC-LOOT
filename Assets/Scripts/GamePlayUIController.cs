using TMPro;
using UnityEngine;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private BarController alertnessBar;
    private Level currentLevel;
    private bool isStopRecordTime;
    private float usedTime;

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
}
