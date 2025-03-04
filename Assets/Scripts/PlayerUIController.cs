using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour, IDataPersistent
{
    [SerializeField] private GameObject Avatar;
    [SerializeField] private GameObject[] ItemList;
    [SerializeField] private int PlayerNum;
    private int PlayerSelectItem = 0;

    public void LoadData(GameState data)
    {
        var playerType = PlayerNum == 1 ? data.GetPlayer1Type() : data.GetPlayer2Type();
        Avatar.GetComponent<Image>().sprite = GameState.characterAvatar[playerType];
        var characterProperty = GameState.charactersData[playerType];
        if (characterProperty.Load == 3)
        {
            ItemList[1].SetActive(true);
            ItemList[2].SetActive(true);
        }
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
    void Awake()
    {
        EventManager.Instance.UpdateInventory += UpdateItem;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateItem(string itemName, int pNum)
    {
        if (pNum == PlayerNum)
        {
            var index = GameState.Instance.GetPlayerItemLoad(pNum);
            var item = ItemList[index].transform.Find("Item");
            item.gameObject.SetActive(true);
            item.GetComponent<Image>().sprite = GameState.chestItem[itemName];
            ChangeItem(itemName, index);
        }
    }

    void ChangeItem(string itemName, int index)
    {
        if (index == PlayerSelectItem)
        {
            EventManager.Instance.TriggerUpdateUserTakenItem(itemName, PlayerNum);
        }
    }
}
