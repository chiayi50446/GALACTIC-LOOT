using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour, IDataPersistent
{
    [SerializeField] private GameObject Avatar;
    [SerializeField] private GameObject[] ItemList;
    [SerializeField] private int PlayerNum;
    [SerializeField] private GameObject CollectItem;

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
        EventManager.Instance.ShowCollectItem += ShowCollectItem;
    }

    void OnDestroy()
    {
        EventManager.Instance.UpdateInventory -= UpdateItem;
        EventManager.Instance.RemoveInventory -= RemoveItem;
        EventManager.Instance.ShowCollectItem -= ShowCollectItem;
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
            if (itemName == "WizardHat") GameState.Instance.SetPlayerDisguiseCount(pNum);
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
            var currentSelectIndex = GameState.Instance.GetPlayerSelectItem(pNum);
            ItemList[currentSelectIndex].transform.Find("select").gameObject.SetActive(false);
            ItemList[currentSelectIndex].transform.Find("Item").gameObject.SetActive(false);

            // Set selectIndex to 0
            currentSelectIndex = 0;
            GameState.Instance.SetPlayerSelectItem(pNum, currentSelectIndex);
            ItemList[currentSelectIndex].transform.Find("select").gameObject.SetActive(true);

            //Move the other items
            for (int i = 0; i < ItemList.Length; i++)
            {
                var item = ItemList[i].transform.Find("Item");
                if (!item.gameObject.activeSelf && i < ItemList.Length - 1)
                {
                    var nextItem = ItemList[i + 1].transform.Find("Item");
                    if (nextItem.gameObject.activeSelf)
                    {
                        item.gameObject.SetActive(true);
                        item.GetComponent<Image>().sprite = nextItem.GetComponent<Image>().sprite;
                        nextItem.gameObject.SetActive(false);

                    }
                }
                if (i == 0)
                {
                    ChangeItemShow(item.GetComponent<Image>().sprite.name, 0);
                }
            }
        }
    }


    void ChangeItem()
    {
        if ((Input.GetKeyDown(KeyCode.C) && PlayerNum == 1) || (Input.GetKeyDown(KeyCode.N) && PlayerNum == 2))
        {
            if (GameState.Instance.GetPlayerItemLoad(PlayerNum) != -1)
            {
                var index = GameState.Instance.GetPlayerSelectItem(PlayerNum);
                ItemList[index].transform.Find("select").gameObject.SetActive(false);
                index = (index + 1) % (GameState.Instance.GetPlayerItemLoad(PlayerNum) + 1);
                ItemList[index].transform.Find("select").gameObject.SetActive(true);
                GameState.Instance.SetPlayerSelectItem(PlayerNum, index);
                ChangeItemShow(ItemList[index].transform.Find("Item").GetComponent<Image>().sprite.name, index);
            }
        }
    }


    void ChangeItemShow(string itemName, int index)
    {
        if (index == GameState.Instance.GetPlayerSelectItem(PlayerNum))
        {
            EventManager.Instance.TriggerUpdateUserTakenItem(itemName, PlayerNum);
        }
    }

    void ShowCollectItem()
    {
        CollectItem.SetActive(true);
    }
}
