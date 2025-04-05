using UnityEngine;

public class ItemController : MonoBehaviour
{
    private GameObject hint;
    // public AudioSource audioSource;
    public AudioClip effectSound;
    protected bool isTrigger1;
    protected bool isTrigger2;
    // Start is called before the first frame update
    public virtual void Start()
    {
        hint = findChildByTag(this.transform, "Hint");
        hint.SetActive(false);
        isTrigger1 = false;
        isTrigger2 = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            hint.SetActive(true);
            if (other.gameObject.name == "Player1")
                isTrigger1 = true;
            else
                isTrigger2 = true;

        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            hint.SetActive(false);
            isTrigger1 = false;
            isTrigger2 = false;
        }
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
