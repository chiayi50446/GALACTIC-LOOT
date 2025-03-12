using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject triggerdItem;
    [SerializeField] private GameObject friendItem;
    [SerializeField] private Sprite originalImage;
    [SerializeField] private Sprite triggeredImage;
    public bool isTrigger;
    private SpriteRenderer spriteRenderer;
    private ButtonController friend;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = originalImage;
        friend = friendItem.GetComponent<ButtonController>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.sprite = triggeredImage;
        isTrigger = true;
        if (triggerdItem.activeSelf)
        {
            triggerdItem.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.sprite = originalImage;
        isTrigger = false;
        if (!triggerdItem.activeSelf && !friend.isTrigger)
        {
            triggerdItem.SetActive(true);
        }
    }
}
