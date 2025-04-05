using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button level2Btn;
    [SerializeField] private bool level2Disable = false;

    void Update()
    {
        if (level2Disable) level2Btn.interactable = false;
        else level2Btn.interactable = true;
    }

    void OnEnable()
    {
        bool isLevel1Clear = GameState.Instance.GetIsLevelClear(Level.Level1);
        level1TimeStar.SetActive(false);
        level1CollectStar.SetActive(false);
        level1SurviveStar.SetActive(false);
        if (isLevel1Clear)
        {
            float usedTime = GameState.Instance.GetClearLevelTime(Level.Level1);
            float targetTime = GameState.ClearTargetTime[Level.Level1];
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
        else
        {
            level2Disable = true;
        }

        bool isLevel2Clear = GameState.Instance.GetIsLevelClear(Level.Level2);

        level2TimeStar.SetActive(false);
        level2CollectStar.SetActive(false);
        level2SurviveStar.SetActive(false);
        if (isLevel2Clear)
        {
            float usedTime = GameState.Instance.GetClearLevelTime(Level.Level2);
            float targetTime = GameState.ClearTargetTime[Level.Level2];
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
        AudioManager.Instance.playButtonSound();
        GameState.Instance.SetCurrentLevel(Level.Level1);
        levelInfoUI.SetActive(true);
        gameObject.SetActive(false);
    }
    public void SelectLevel2()
    {
        AudioManager.Instance.playButtonSound();
        GameState.Instance.SetCurrentLevel(Level.Level2);
        levelInfoUI.SetActive(true);
        gameObject.SetActive(false);
    }
    public void QuitLevelSelect()
    {
        AudioManager.Instance.playButtonSound();
        startMenuUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
