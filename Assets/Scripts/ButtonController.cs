using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject triggerdItem;
    [SerializeField] private GameObject friendItem;
    [SerializeField] private Sprite originalImage;
    [SerializeField] private Sprite triggeredImage;
    [SerializeField] private bool isTriggerDisplay = false;
    public bool isTrigger;
    private SpriteRenderer spriteRenderer;
    private ButtonController friend;
    private ArrayList arrTriggerObjects;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = originalImage;
        arrTriggerObjects = new ArrayList();
        if (friendItem != null)
            friend = friendItem.GetComponent<ButtonController>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        arrTriggerObjects.Add(collision.name);
        spriteRenderer.sprite = triggeredImage;
        isTrigger = true;
        if (triggerdItem.activeSelf == !isTriggerDisplay)
        {
            triggerdItem.SetActive(isTriggerDisplay);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        arrTriggerObjects.Remove(collision.name);
        if (arrTriggerObjects.Count == 0)
        {
            isTrigger = false;
            spriteRenderer.sprite = originalImage;
            if (triggerdItem.activeSelf == isTriggerDisplay && (friendItem == null || !friend.isTrigger))
            {
                triggerdItem.SetActive(!isTriggerDisplay);
            }
        }
    }
}
