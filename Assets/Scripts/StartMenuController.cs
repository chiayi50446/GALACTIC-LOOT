using UnityEngine;

public class StartMenuController : MenuController
{
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private GameObject selectLevelUI;
    public void StartGame()
    {
        Debug.Log("Start Game");
        GameState.Instance.SetCurrentLevel(1);
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
