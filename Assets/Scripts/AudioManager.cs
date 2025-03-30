using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSourceBGM;
    [SerializeField]
    private AudioSource audioSourceSE;
    [SerializeField]
    private AudioClip buttonSound;

    [SerializeField]
    private AudioClip titleBGM;

    [SerializeField]
    private AudioClip Level1NormalBGM;

    [SerializeField]
    private AudioClip Level1BossBGM;

    [SerializeField]
    private AudioClip Level2NormalBGM;

    [SerializeField]
    private AudioClip Level2BossBGM;
    [SerializeField]
    private AudioClip GameClearBGM;
    [SerializeField]
    private AudioClip GameOverBGM;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void playButtonSound()
    {
        audioSourceSE.PlayOneShot(buttonSound);
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(titleBGM);
    }

    public void PlayLevel1NormalMusic()
    {
        PlayMusic(Level1NormalBGM);
    }

    public void PlayLevel1BossMusic()
    {
        PlayMusic(Level1BossBGM);
    }

    public void PlayLevel2NormalMusic()
    {
        PlayMusic(Level2NormalBGM);
    }

    public void PlayLevel2BossMusic()
    {
        PlayMusic(Level2BossBGM);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSourceBGM.clip != clip)
        {
            audioSourceBGM.clip = clip;
            audioSourceBGM.Play();
        }
    }
}
