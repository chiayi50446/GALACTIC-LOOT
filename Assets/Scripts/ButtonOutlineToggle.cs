using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutlineToggle : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Outline outline;
    [SerializeField]
    private CharacterType type;

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null && type != CharacterType.type1)
        {
            outline.enabled = false; // 預設隱藏
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        EventManager.instance.SelectCharacterActive(type);
        if (outline != null)
        {
            outline.enabled = true; // 選擇時顯示
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = false; // 取消選擇時隱藏
        }
    }
}
