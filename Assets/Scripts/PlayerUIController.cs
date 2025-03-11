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
        var playerType = data.GetPlayerType(PlayerNum);
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
        EventManager.Instance.RemoveInventory += RemoveItem;
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

    void RemoveItem(int pNum)
    {
        if (pNum == PlayerNum)
        {
            //Remove Current item
            GameState.Instance.ReducePlayerItemLoad(pNum);
            GameState.Instance.SetPlayerSelectItem(pNum, 0);
            var index = GameState.Instance.GetPlayerSelectItem(pNum);
            var item = ItemList[index].transform.Find("Item");
            item.gameObject.SetActive(false);

            //Move the other items
            for (int i = 0; i < ItemList.Length; i++)
            {
                item = ItemList[i].transform.Find("Item");
                if (!item.gameObject.activeSelf && i < ItemList.Length - 1)
                {
                    var nextItem = ItemList[i + 1].transform.Find("Item");
                    if (nextItem.gameObject.activeSelf)
                    {
                        item.gameObject.SetActive(true);
                        item.GetComponent<Image>().sprite = nextItem.GetComponent<Image>().sprite;
                        nextItem.gameObject.SetActive(false);
                        if (i == 0)
                        {
                            ChangeItem(item.GetComponent<Image>().sprite.name, 0);
                        }
                    }
                }
            }
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
