using UnityEngine;

public class SelectLevelMenuController : MenuController
{
    [SerializeField]
    private GameObject levelInfoUI;
    [SerializeField]
    private GameObject startMenuUI;
    [SerializeField] private GameObject level1TimeStar;
    [SerializeField] private GameObject level1CollectStar;
    [SerializeField] private GameObject level1SurviveStar;
    [SerializeField] private GameObject level2TimeStar;
    [SerializeField] private GameObject level2CollectStar;
    [SerializeField] private GameObject level2SurviveStar;

    void OnEnable()
    {
        bool isLevel1Clear = GameState.Instance.GetIsLevelClear(Level.Level1);
        level1TimeStar.SetActive(false);
        level1CollectStar.SetActive(false);
        level1SurviveStar.SetActive(false);
        if (isLevel1Clear)
        {
            float usedTime = GameState.Instance.GetClearLevelTime(Level.Level1);
            float targetTime = GameState.clearTargetTime[Level.Level1];
            if (usedTime <= targetTime)
            {
                level1TimeStar.SetActive(true);
            }

            if (GameState.Instance.GetIsCollectItemGet(Level.Level1))
            {
                level1CollectStar.SetActive(true);
            }
            if (GameState.Instance.GetPlayerDeathNum(Level.Level1) == 0)
            {
                level1SurviveStar.SetActive(true);
            }
        }

        bool isLevel2Clear = GameState.Instance.GetIsLevelClear(Level.Level2);

        level2TimeStar.SetActive(false);
        level2CollectStar.SetActive(false);
        level2SurviveStar.SetActive(false);
        if (isLevel2Clear)
        {
            float usedTime = GameState.Instance.GetClearLevelTime(Level.Level2);
            float targetTime = GameState.clearTargetTime[Level.Level2];
            if (usedTime <= targetTime)
            {
                level2TimeStar.SetActive(true);
            }

            if (GameState.Instance.GetIsCollectItemGet(Level.Level2))
            {
                level2CollectStar.SetActive(true);
            }
            if (GameState.Instance.GetPlayerDeathNum(Level.Level2) == 0)
            {
                level2SurviveStar.SetActive(true);
            }
        }
    }
    public void SelectLevel1()
    {
        Debug.Log("Select Level1");
        GameState.Instance.SetCurrentLevel(Level.Level1);
        GameState.Instance.ResetCurrentLevel(Level.Level1);
        levelInfoUI.SetActive(true);
        gameObject.SetActive(false);
    }
    public void SelectLevel2()
    {
        Debug.Log("Select Level2");
        GameState.Instance.SetCurrentLevel(Level.Level2);
        GameState.Instance.ResetCurrentLevel(Level.Level2);
        levelInfoUI.SetActive(true);
        gameObject.SetActive(false);
    }
    public void QuitLevelSelect()
    {
        startMenuUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
