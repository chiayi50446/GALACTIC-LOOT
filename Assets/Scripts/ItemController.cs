using UnityEngine;

public class ItemController : MonoBehaviour
{
    private GameObject hint;
    // public AudioSource audioSource;
    public AudioClip effectSound;
    protected bool isTrigger;
    // Start is called before the first frame update
    public virtual void Start()
    {
        hint = findChildByTag(this.transform, "Hint");
        hint.SetActive(false);
        isTrigger = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isTrigger)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                hint.SetActive(false);
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            hint.SetActive(true);
            isTrigger = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        hint.SetActive(false);
        isTrigger = false;
    }

    protected GameObject findChildByTag(Transform parent, string inputTag)
    {
        GameObject childWithTag = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag(inputTag))
            {
                childWithTag = parent.GetChild(i).gameObject;
                break;
            }
        }

        return childWithTag;
    }
}
