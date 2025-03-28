using UnityEngine;

public class StartMenuController : MenuController
{
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private GameObject selectLevelUI;
    [SerializeField]
    private GameObject selectCharacterUI;

    void Awake()
    {
        if (GameState.Instance.GetCurrentLevel() == Level.Level2)
        {
            selectCharacterUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    public void StartGame()
    {
        Debug.Log("Start Game");
        GameState.Instance.SetCurrentLevel(Level.Level1);
        tutorialUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        Debug.Log("Resume Game");
        selectLevelUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
