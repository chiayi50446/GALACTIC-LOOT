using UnityEngine;

public class StartMenuController : MenuController
{
    [SerializeField]
    private GameObject selectCharacterUI;
    [SerializeField]
    private GameObject selectLevelUI;
    public void StartGame()
    {
        Debug.Log("Start Game");
        GameState.Instance.SetCurrentLevel(1);
        selectCharacterUI.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        Debug.Log("Resume Game");
        selectLevelUI.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
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
