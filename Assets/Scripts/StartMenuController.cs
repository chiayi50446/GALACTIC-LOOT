using UnityEngine;

public class StartMenuController : MenuController
{
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private GameObject selectLevelUI;
    [SerializeField]
    private GameObject selectCharacterUI;
    [SerializeField]
    private GameObject settingUI;

    void Awake()
    {
        if (GameState.Instance.GetCurrentLevel() == Level.Level2)
        {
            selectCharacterUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        AudioManager.Instance.PlayMainMenuMusic();
    }

    public void StartGame()
    {
        AudioManager.Instance.playButtonSound();
        GameState.Instance.SetCurrentLevel(Level.Level1);
        tutorialUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        AudioManager.Instance.playButtonSound();
        selectLevelUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        AudioManager.Instance.playButtonSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void SettingGame()
    {
        AudioManager.Instance.playButtonSound();
        settingUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
