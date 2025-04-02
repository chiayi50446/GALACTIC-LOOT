using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutlineToggle : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Outline outline;
    [SerializeField]
    private CharacterType type;
    private Animator animator;
    private Image img;
    private Sprite originalSprite;

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null && type != CharacterType.type1)
        {
            outline.enabled = false; // 預設隱藏
        }

        animator = GetComponent<Animator>();
        if (animator != null && type != CharacterType.type1)
        {
            animator.enabled = false;
        }

        img = GetComponent<Image>();
        originalSprite = img.sprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        EventManager.Instance.SelectCharacterActive(type);
        if (outline != null)
        {
            outline.enabled = true; // 選擇時顯示
        }
        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = false; // 取消選擇時隱藏
        }
        if (animator != null)
        {
            animator.enabled = false;
            img.sprite = originalSprite;
        }
    }
}
