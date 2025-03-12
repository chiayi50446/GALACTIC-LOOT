using UnityEngine;

public class GamePlayUIController : MonoBehaviour
{
    private BarController alertnessBar;
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
}
