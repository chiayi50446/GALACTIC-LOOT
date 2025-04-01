using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSourceBGM;
    [SerializeField]
    private AudioSource audioSourceSE;

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
    private AudioClip GameOverBGM;
    [SerializeField]
    private AudioClip buttonSound;
    [SerializeField]
    private AudioClip moveSelectSound;
    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private AudioClip throwSound;
    [SerializeField]
    private AudioClip bombSound;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip hurtSound;
    [SerializeField]
    private AudioClip enemyVisionSound;
    [SerializeField]
    private AudioClip diceSound;
    [SerializeField]
    private AudioClip equipItemSound;
    [SerializeField]
    private AudioClip unEquipItemSound;
    [SerializeField]
    private AudioClip collectibleItemSound;
    [SerializeField]
    private AudioClip pushSound;
    [SerializeField]
    private AudioClip stopSound;
    [SerializeField]
    private AudioClip bossNormalAttackSound;
    [SerializeField]
    private AudioClip bossMagicAttackSound;
    [SerializeField]
    private AudioClip enterDoorSound;
    [SerializeField]
    private AudioClip moveCameraSound;
    [SerializeField]
    private AudioClip pressFloorButtonSound;
    [SerializeField]
    private AudioClip releaseFloorButtonSound;
    [SerializeField]
    private AudioClip dialogueSound;


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

    public void PlayGameOverMusic()
    {
        PlayMusic(GameOverBGM);
    }

    public void playButtonSound()
    {
        audioSourceSE.PlayOneShot(buttonSound);
    }

    public void playMoveSelectSound()
    {
        audioSourceSE.PlayOneShot(moveSelectSound);
    }

    public void playShootSound()
    {
        audioSourceSE.PlayOneShot(shootSound);
    }

    public void playThrowSound()
    {
        audioSourceSE.PlayOneShot(throwSound);
    }

    public void playBombSound()
    {
        audioSourceSE.PlayOneShot(bombSound);
    }

    public void playHitSound()
    {
        audioSourceSE.PlayOneShot(hitSound);
    }

    public void playHurtSound()
    {
        audioSourceSE.PlayOneShot(hurtSound);
    }

    public void playEnemyVisionSound()
    {
        audioSourceSE.PlayOneShot(enemyVisionSound);
    }

    public void playDiceSound()
    {
        audioSourceSE.PlayOneShot(diceSound);
    }

    public void playEquipSound()
    {
        audioSourceSE.PlayOneShot(equipItemSound);
    }

    public void playUnEquipSound()
    {
        audioSourceSE.PlayOneShot(unEquipItemSound);
    }

    public void playCollectSound()
    {
        audioSourceSE.PlayOneShot(collectibleItemSound);
    }

    public void playPushSound()
    {
        audioSourceSE.PlayOneShot(pushSound);
    }

    public void playStopSound()
    {
        audioSourceSE.PlayOneShot(stopSound);
    }

    public void playBossNormalAttackSound()
    {
        audioSourceSE.PlayOneShot(bossNormalAttackSound);
    }

    public void playBossMagicAttackSound()
    {
        audioSourceSE.PlayOneShot(bossMagicAttackSound);
    }

    public void playEnterDoorSound()
    {
        audioSourceSE.PlayOneShot(enterDoorSound);
    }

    public void playMoveCameraSound()
    {
        audioSourceSE.PlayOneShot(moveCameraSound);
    }

    public void playPressFloorButtonSound()
    {
        audioSourceSE.PlayOneShot(pressFloorButtonSound);
    }

    public void playReleaseFloorButtonSound()
    {
        audioSourceSE.PlayOneShot(releaseFloorButtonSound);
    }

    public void playDialogueSound()
    {
        audioSourceSE.PlayOneShot(dialogueSound);
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
