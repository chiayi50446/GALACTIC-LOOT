using UnityEngine;
using TMPro;

public class LevelInfoController : MenuController
{
    [SerializeField]
    private GameObject selectCharacterUI;
    [SerializeField]
    private GameObject selectLevelUI;
    [SerializeField] private GameObject levelTimeStar;
    [SerializeField] private GameObject levelCollectStar;
    [SerializeField] private GameObject levelSurviveStar;
    [SerializeField] private TMP_Text targetTimeText;
    [SerializeField] private TMP_Text usedTimeText;
    void OnEnable()
    {
        var currentLevel = GameState.Instance.GetCurrentLevel();
        levelTimeStar.SetActive(false);
        levelCollectStar.SetActive(false);
        levelSurviveStar.SetActive(false);

        float targetTime = GameState.clearTargetTime[currentLevel];
        int targetMinutes = Mathf.FloorToInt(targetTime / 60);
        int targetSeconds = Mathf.FloorToInt(targetTime % 60);
        targetTimeText.text = string.Format("{0:00}:{1:00}", targetMinutes, targetSeconds);
        usedTimeText.text = string.Format("{0:00}:{1:00}", 0, 0);

        bool isLevelClear = GameState.Instance.GetIsLevelClear(currentLevel);
        if (isLevelClear)
        {
            float usedTime = GameState.Instance.GetClearLevelTime(currentLevel);
            if (usedTime <= targetTime)
            {
                levelTimeStar.SetActive(true);
            }

            if (GameState.Instance.GetIsCollectItemGet(currentLevel))
            {
                levelCollectStar.SetActive(true);
            }
            if (GameState.Instance.GetPlayerDeathNum(currentLevel) == 0)
            {
                levelSurviveStar.SetActive(true);
            }

            int usedMinutes = Mathf.FloorToInt(usedTime / 60);
            int usedSeconds = Mathf.FloorToInt(usedTime % 60);
            usedTimeText.text = string.Format("{0:00}:{1:00}", usedMinutes, usedSeconds);


        }
    }

    public void OK()
    {
        selectCharacterUI.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Quit()
    {
        selectLevelUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
