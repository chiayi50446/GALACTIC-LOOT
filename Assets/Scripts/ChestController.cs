
using UnityEngine;
using UnityEngine.UI;

public class ChestController : ItemController, IDataPersistent
{
    private Animator anim;
    private bool isOpen;
    private GameObject chestItem;
    private Level currentLevel;

    public override void Start()
    {
        base.Start();
        isOpen = false;
        chestItem = findChildByTag(this.transform, "chestItem");
        currentLevel = GameState.Instance.GetCurrentLevel();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isOpen)
        {
            var itemName = chestItem.transform.GetChild(0).name;
            if (Input.GetKeyDown(KeyCode.E) && isTrigger1)
            {
                isOpen = true;
                anim.SetBool("Open", true);
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(true); }, 0.8f));
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(false); }, 1.3f));
                if (itemName == "Beer")
                {
                    AudioManager.Instance.playCollectSound();
                    GetComponent<Collider2D>().enabled = false;
                    GameState.Instance.SetIsCollectItemGet(currentLevel);
                    EventManager.Instance.TriggerShowCollectItem();
                }
                else
                {
                    if (!GameState.Instance.IsPlayerItemFull(1))
                    {
                        AudioManager.Instance.playEquipSound();
                        GetComponent<Collider2D>().enabled = false;
                        StartCoroutine(Helper.Delay(() => { EventManager.Instance.TriggerUpdateInventory(itemName, 1); }, 1.3f));
                    }
                    else
                    {
                        AudioManager.Instance.playUnEquipSound();
                        isOpen = false;
                        StartCoroutine(Helper.Delay(() => { anim.SetBool("Open", false); }, 1.3f));
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.U) && isTrigger2)
            {
                isOpen = true;
                anim.SetBool("Open", true);
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(true); }, 0.8f));
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(false); }, 1.3f));

                if (itemName == "Beer")
                {
                    AudioManager.Instance.playCollectSound();
                    GetComponent<Collider2D>().enabled = false;
                    GameState.Instance.SetIsCollectItemGet(currentLevel);
                    EventManager.Instance.TriggerShowCollectItem();
                }
                else
                {
                    if (!GameState.Instance.IsPlayerItemFull(2))
                    {
                        AudioManager.Instance.playEquipSound();
                        GetComponent<Collider2D>().enabled = false;
                        StartCoroutine(Helper.Delay(() => { EventManager.Instance.TriggerUpdateInventory(itemName, 2); }, 1.3f));
                    }
                    else
                    {
                        AudioManager.Instance.playUnEquipSound();
                        isOpen = false;
                        StartCoroutine(Helper.Delay(() => { anim.SetBool("Open", false); }, 1.3f));
                    }
                }
            }
        }
    }

    public void LoadData(GameState data)
    {
    }

    public void SaveData()
    {

    }
}
