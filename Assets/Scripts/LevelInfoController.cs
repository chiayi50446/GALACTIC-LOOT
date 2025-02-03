using UnityEngine;

public class LevelInfoController : MenuController
{
    [SerializeField]
    private GameObject selectCharacterUI;
    [SerializeField]
    private GameObject selectLevelUI;

    public void OK()
    {
        selectCharacterUI.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void Quit()
    {
        selectLevelUI.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
