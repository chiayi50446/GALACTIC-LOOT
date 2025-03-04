
using UnityEngine;
using UnityEngine.UI;

public class ChestController : ItemController, IDataPersistent
{
    private Animator anim;
    private bool isOpen;
    private GameObject chestItem;

    [SerializeField]
    public Text displayText;
    public override void Start()
    {
        base.Start();
        isOpen = false;
        chestItem = findChildByTag(this.transform, "chestItem");
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
            if (Input.GetKeyDown(KeyCode.RightControl) && isTrigger1)
            {
                isOpen = true;
                anim.SetBool("Open", true);
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(true); }, 0.8f));
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(false); }, 1.3f));
                GetComponent<Collider2D>().enabled = false;
                StartCoroutine(Helper.Delay(() => { EventManager.instance.TriggerUpdateInventory(chestItem.transform.GetChild(0).name, 1); }, 1.3f));
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
            if (Input.GetKeyDown(KeyCode.LeftControl) && isTrigger2)
            {
                isOpen = true;
                anim.SetBool("Open", true);
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(true); }, 0.8f));
                StartCoroutine(Helper.Delay(() => { chestItem.SetActive(false); }, 1.3f));
                GetComponent<Collider2D>().enabled = false;

                StartCoroutine(Helper.Delay(() => { EventManager.instance.TriggerUpdateInventory(chestItem.transform.GetChild(0).name, 2); }, 1.3f));

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

    private void UpdateGemCount()
    {
        // int gemCount = GameState.Instance.GetGemCount() + 1;
        // GameState.Instance.SetGemCount(gemCount);
        // displayText.text = gemCount.ToString();
    }

    private void UpdateRuneCount()
    {
        // GameState.Instance.SetIsRuneStone(true);
        // displayText.text = "1";
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
