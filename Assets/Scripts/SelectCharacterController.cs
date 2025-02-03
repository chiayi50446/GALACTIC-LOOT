using UnityEngine;

public class SelectCharacterController : MenuController
{
    [SerializeField]
    private GameObject startMenu;
    public void OK()
    {
        DataPersistentManager.instance.LoadGame();
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        startMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
