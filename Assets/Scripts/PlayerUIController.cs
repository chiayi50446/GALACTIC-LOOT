using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour, IDataPersistent
{
    [SerializeField] private GameObject Avatar;
    [SerializeField] private GameObject[] ItemList;
    [SerializeField] private int PlayerNum;

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

    void OnDestroy()
    {
        EventManager.Instance.UpdateInventory -= UpdateItem;
        EventManager.Instance.RemoveInventory -= RemoveItem;
    }

    void Update()
    {
        ChangeItem();
    }

    void UpdateItem(string itemName, int pNum)
    {
        if (pNum == PlayerNum)
        {
            var index = GameState.Instance.AddPlayerItemLoad(pNum);
            var item = ItemList[index].transform.Find("Item");
            item.gameObject.SetActive(true);
            item.GetComponent<Image>().sprite = GameState.chestItem[itemName];
            ChangeItemShow(itemName, index);
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
                            ChangeItemShow(item.GetComponent<Image>().sprite.name, 0);
                        }
                    }
                }
            }
        }
    }


    void ChangeItem()
    {
        if ((Input.GetKeyDown(KeyCode.C) && PlayerNum == 1) || (Input.GetKeyDown(KeyCode.N) && PlayerNum == 2))
        {
            var index = GameState.Instance.GetPlayerSelectItem(PlayerNum);
            ItemList[index].transform.Find("select").gameObject.SetActive(false);
            index = (index + 1) % (GameState.Instance.GetPlayerItemLoad(PlayerNum) + 1);
            ItemList[index].transform.Find("select").gameObject.SetActive(true);
            GameState.Instance.SetPlayerSelectItem(PlayerNum, index);
            ChangeItemShow(ItemList[index].transform.Find("Item").GetComponent<Image>().sprite.name, index);
        }
    }


    void ChangeItemShow(string itemName, int index)
    {
        if (index == GameState.Instance.GetPlayerSelectItem(PlayerNum))
        {
            EventManager.Instance.TriggerUpdateUserTakenItem(itemName, PlayerNum);
        }
    }
}
