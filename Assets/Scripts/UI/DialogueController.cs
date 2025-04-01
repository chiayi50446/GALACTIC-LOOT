using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image dialogueAvatar;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;
    public float typeSpeed = 0.1f;
    private int dialogueIndex = 0;
    private int enemyDice;
    private int playerDice;
    private Level currentLevel;
    private bool isSpeedUp = false;

    private List<Dialogue> initialText = new List<Dialogue>(){
        new Dialogue(DialogueType.Enemy, "Who are you?\nYou shouldn't be here!", false),
        new Dialogue(DialogueType.Player, "......\n(Try to persuade the guard!)", false),
        new Dialogue(DialogueType.Player, "Start Nigotiation!", false),
    };

    private List<Dialogue> nigotiationText = new List<Dialogue>();

    void Awake()
    {
        // nextButton.onClick.AddListener(nextDialog);
        currentLevel = GameState.Instance.GetCurrentLevel();
    }
    private void OnEnable()
    {
        EventManager.Instance.ActiveNegotiationCheck += StartNigotiation;
    }

    private void OnDisable()
    {
        EventManager.Instance.ActiveNegotiationCheck -= StartNigotiation;
    }

    void Update()
    {
        if (nextButton.IsActive())
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                AudioManager.Instance.playButtonSound();
                nextDialog();
            }
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && dialoguePanel.activeSelf)
            {
                isSpeedUp = true;
            }
        }
    }

    void StartNigotiation()
    {
        dialoguePanel.SetActive(true);
        Time.timeScale = 0;
        dialogueIndex = 0;
        nextButton.gameObject.SetActive(false);
        nigotiationText.AddRange(initialText);

        enemyDice = Random.Range(0, 60) % 6 + 1;
        playerDice = Random.Range(0, 60) % 6 + 1 + GameState.charactersData[GameState.Instance.GetLastAlertPlayerType()].Charisma;
        nigotiationText.Add(new Dialogue(DialogueType.Enemy, "Enemy's negotiation dice is " + enemyDice + " (1D6)", true));
        nigotiationText.Add(new Dialogue(DialogueType.Player, "Player's negotiation dice is " + playerDice + " (1D6 + Charisma)", true));
        if (enemyDice > playerDice)
        {
            nigotiationText.Add(new Dialogue(DialogueType.Enemy, "You are under arrest!\n(Nigotiation fail)", false));
        }
        else
        {
            nigotiationText.Add(new Dialogue(DialogueType.Enemy, "Don't let me catch you sneaking around again.\n(Nigotiation success)", false));
        }
        startDialogue();
    }

    void startDialogue()
    {
        nextButton.gameObject.SetActive(false);
        StartCoroutine(ShowText(nigotiationText[dialogueIndex]));
    }

    public void nextDialog()
    {
        dialogueIndex++;
        if (dialogueIndex < nigotiationText.Count)
        {
            startDialogue();
        }
        else
        {
            if (enemyDice > playerDice)
            {
                GameState.Instance.SetIsLevelClear(currentLevel, false);
                DataPersistentManager.instance.EndGame();
                closeDialogue();
            }
            else
            {
                GameState.Instance.SetAlertnessLevel(GameState.Instance.GetAlertnessLevel() - 1);
                EventManager.Instance.TriggerUpdateAlertnessLevel();
                closeDialogue();
            }
        }
    }

    void closeDialogue()
    {
        nigotiationText.Clear();
        dialoguePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private IEnumerator ShowText(Dialogue dialog)
    {
        if (dialog.type == DialogueType.Enemy)
        {
            dialogueAvatar.sprite = GameState.enemyAvatar[GameState.Instance.GetLastAlertEnemyType()];
        }
        else if (dialog.type == DialogueType.Player)
        {
            dialogueAvatar.sprite = GameState.characterAvatar[GameState.Instance.GetLastAlertPlayerType()];
        }

        dialogueText.text = "";
        dialogueText.gameObject.SetActive(true);

        if (dialog.isPlaySound)
        {
            AudioManager.Instance.playDiceSound();
        }

        for (int i = 0; i < dialog.text.Length; i++)
        {
            if (isSpeedUp)
            {
                dialogueText.text = dialog.text;

                isSpeedUp = false;
                break;
            }
            dialogueText.text += dialog.text[i];

            if (i % 3 == 0)
            {
                AudioManager.Instance.playDialogueSound();
            }

            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        nextButton.gameObject.SetActive(true);
        nextButton.GetComponent<Button>().Select();
    }
}
