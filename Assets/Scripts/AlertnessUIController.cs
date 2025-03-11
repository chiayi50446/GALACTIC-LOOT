using UnityEngine;

public class AlertnessUIController : MonoBehaviour
{
    private BarController alertnessBar;
    void Awake()
    {
        EventManager.Instance.UpdateAlertnessLevel += UpdateAlertnessLevel;
    }

    void OnDestroy()
    {
        EventManager.Instance.UpdateAlertnessLevel -= UpdateAlertnessLevel;
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
        float nowAlertnessLevel = alertnessBar.GetValue();
        if (nowAlertnessLevel == 3)
        {
            Debug.Log("Alertness Level = 3!!!");
            return;
        }
        alertnessBar.SetValue(nowAlertnessLevel + 1);
        GameState.Instance.SetAlertnessLevel(nowAlertnessLevel + 1);
        EventManager.Instance.TriggerUpdateVision();
    }
}
