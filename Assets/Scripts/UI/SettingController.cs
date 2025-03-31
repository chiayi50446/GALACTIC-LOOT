using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private AudioSource bgm;
    [SerializeField]
    private AudioSource se;
    [SerializeField]
    private Button bgmButton;
    [SerializeField]
    private Button seButton;
    [SerializeField]
    private Sprite onImage;
    [SerializeField]
    private Sprite offImage;

    void Start()
    {
        bgmButton.onClick.AddListener(() => { ToggleMute(bgm, bgmButton); });
        seButton.onClick.AddListener(() => { ToggleMute(se, seButton); });
    }

    public void ToggleMute(AudioSource audioSource, Button btn)
    {
        AudioManager.Instance.playButtonSound();
        audioSource.mute = !audioSource.mute;
        if (audioSource.mute)
        {
            btn.image.sprite = offImage;
        }
        else
        {
            btn.image.sprite = onImage;
        }
    }

    public void Quit()
    {
        AudioManager.Instance.playButtonSound();
        menuUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
