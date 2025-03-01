using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject triggerdItem;
    [SerializeField] private Sprite originalImage;
    [SerializeField] private Sprite triggeredImage;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = originalImage;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.sprite = triggeredImage;
        triggerdItem.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.sprite = originalImage;
        triggerdItem.SetActive(true);

    }
}
