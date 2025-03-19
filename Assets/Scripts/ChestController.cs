
using UnityEngine;
using UnityEngine.UI;

public class ChestController : ItemController, IDataPersistent
{
    private Animator anim;
    private bool isOpen;
    private GameObject chestItem;
    private Level currentLevel;

    // [SerializeField] public Text displayText;
    [SerializeField] private GameObject displayGuide;
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
    public override void Update()
    {
        if (!isOpen)
        {
            base.Update();
            var itemName = chestItem.transform.GetChild(0).name;
            if (Input.GetKeyDown(KeyCode.E) && isTrigger1)
            {
                isOpen = true;
                anim.SetBool("Open", true);
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(true); }, 0.8f));
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(false); }, 1.3f));
                if (itemName == "Beer")
                {
                    GetComponent<Collider2D>().enabled = false;
                    GameState.Instance.SetIsCollectItemGet(currentLevel);
                }
                else
                {
                    if (!GameState.Instance.IsPlayerItemFull(1))
                    {
                        GetComponent<Collider2D>().enabled = false;
                        StartCoroutine(Helper.Delay(() => { EventManager.Instance.TriggerUpdateInventory(itemName, 1); }, 1.3f));
                        if (displayGuide != null)
                        {
                            StartCoroutine(Helper.Delay(() => { EventManager.Instance.TriggerDisplayGuide(displayGuide); }, 1.4f));
                        }
                    }
                    else
                    {
                        isOpen = false;
                        StartCoroutine(Helper.Delay(() => { anim.SetBool("Open", false); }, 1.3f));
                    }
                }
                // GameState.Instance.SetChestBoxName(name);
                // audioSource.PlayOneShot(effectSound);
                // if (tag == "gemChest")
                // {
                //     UpdateGemCount();
                // }
                // if (tag == "runeChest")
                // {
                //     UpdateRuneCount();
                // }
            }
            if (Input.GetKeyDown(KeyCode.U) && isTrigger2)
            {
                isOpen = true;
                anim.SetBool("Open", true);
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(true); }, 0.8f));
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(false); }, 1.3f));

                if (itemName == "Beer")
                {
                    GetComponent<Collider2D>().enabled = false;
                    GameState.Instance.SetIsCollectItemGet(currentLevel);
                }
                else
                {
                    if (!GameState.Instance.IsPlayerItemFull(2))
                    {
                        GetComponent<Collider2D>().enabled = false;
                        StartCoroutine(Helper.Delay(() => { EventManager.Instance.TriggerUpdateInventory(itemName, 2); }, 1.3f));
                        if (displayGuide != null)
                        {
                            StartCoroutine(Helper.Delay(() => { EventManager.Instance.TriggerDisplayGuide(displayGuide); }, 1.4f));
                        }
                    }
                    else
                    {
                        isOpen = false;
                        StartCoroutine(Helper.Delay(() => { anim.SetBool("Open", false); }, 1.3f));
                    }
                }


                // GameState.Instance.SetChestBoxName(name);
                // audioSource.PlayOneShot(effectSound);
                // if (tag == "gemChest")
                // {
                //     UpdateGemCount();
                // }
                // if (tag == "runeChest")
                // {
                //     UpdateRuneCount();
                // }
            }
        }
    }

    public void LoadData(GameState data)
    {
        // if (GameState.Instance.CheckChestBoxNameExist(name))
        // {
        //     isOpen = true;
        //     anim.SetBool("IsOpened", true);
        //     GetComponent<Collider2D>().enabled = false;
        // }
        // if (tag == "gemChest")
        // {
        //     displayText.text = GameState.Instance.GetGemCount().ToString();
        // }
        // if (tag == "runeChest" && GameState.Instance.GetIsRuneStone())
        // {
        //     displayText.text = "1";
        // }
    }

    public void SaveData()
    {

    }
}
