using TMPro;
using UnityEngine;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private BarController alertnessBar;
    float usedTime;

    void Awake()
    {
        EventManager.Instance.UpdateAlertnessLevel += UpdateAlertnessLevel;
        EventManager.Instance.ActiveBossHealth += OpenBossRoomUI;
    }

    void OnDestroy()
    {
        EventManager.Instance.UpdateAlertnessLevel -= UpdateAlertnessLevel;
        EventManager.Instance.ActiveBossHealth -= OpenBossRoomUI;
    }

    void Start()
    {
        alertnessBar = GetComponentInChildren<BarController>();
        alertnessBar.SetValue(0);
        GameState.Instance.SetAlertnessLevel(0);
        EventManager.Instance.TriggerUpdateVision();
        usedTime = 0;
    }

    void Update()
    {
        UpdateTimer();
    }

    private void UpdateAlertnessLevel()
    {
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
        usedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(usedTime / 60);
        int seconds = Mathf.FloorToInt(usedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
