using UnityEngine;

public class SelectCharacterController : MenuController
{
    [SerializeField]
    private GameObject startMenu;
    public void OK()
    {
        DataPersistentManager.instance.LoadGame();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Quit()
    {
        startMenu.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
