using UnityEngine;

public class SelectLevelMenuController : MenuController
{
    [SerializeField]
    private GameObject levelInfoUI;

    public void SelectLevel1()
    {
        Debug.Log("Select Level1");
        GameState.Instance.SetCurrentLevel(1);
        levelInfoUI.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void SelectLevel2()
    {
        Debug.Log("Select Level2");
        GameState.Instance.SetCurrentLevel(2);
        levelInfoUI.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
