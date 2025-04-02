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
    [SerializeField] private GameObject diceView;
    [SerializeField] private GameObject dice;
    [SerializeField] private Sprite[] diceSprites;
    public float typeSpeed = 0.1f;
    private int dialogueIndex = 0;
    private int enemyDice;
    private int playerDice;
    private Level currentLevel;
    private bool isSpeedUp = false;
    private Animator diceAnim;
    private int textCount;

    private List<Dialogue> initialText = new List<Dialogue>(){
        new Dialogue(DialogueType.Enemy, "Who are you?\nYou shouldn't be here!", false),
        new Dialogue(DialogueType.Player, "......\n(Try to persuade the guard!)", false),
        new Dialogue(DialogueType.Player, "Start Negotiation!", false),
    };

    private List<Dialogue> negotiationText = new List<Dialogue>();

    void Awake()
    {
        // nextButton.onClick.AddListener(nextDialog);
        currentLevel = GameState.Instance.GetCurrentLevel();
        diceAnim = dice.GetComponent<Animator>();
        diceAnim.updateMode = AnimatorUpdateMode.UnscaledTime;  // 讓動畫使用 Unscaled Time
        diceView.SetActive(false);
    }
    private void OnEnable()
    {
        EventManager.Instance.ActiveNegotiationCheck += StartNegotiation;
    }

    private void OnDisable()
    {
        EventManager.Instance.ActiveNegotiationCheck -= StartNegotiation;
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

    void StartNegotiation()
    {
        dialoguePanel.SetActive(true);
        Time.timeScale = 0;
        dialogueIndex = 0;
        nextButton.gameObject.SetActive(false);
        negotiationText.AddRange(initialText);

        var charisma = GameState.charactersData[GameState.Instance.GetLastAlertPlayerType()].Charisma;
        enemyDice = Random.Range(0, 60) % 6 + 1;
        playerDice = Random.Range(0, 60) % 6 + 1 + charisma;
        negotiationText.Add(new Dialogue(DialogueType.Enemy, "Enemy's negotiation dice is " + enemyDice + " (1D6)", true, enemyDice));
        negotiationText.Add(new Dialogue(DialogueType.Player, "Player's negotiation dice is " + playerDice + " (1D6 <color=yellow>" + (playerDice - charisma) + "</color> + Charisma <color=yellow>" + charisma + "</color>)", true, (playerDice - charisma)));
        if (enemyDice > playerDice)
        {
            negotiationText.Add(new Dialogue(DialogueType.Enemy, "You are under arrest!\n(Negotiation fail)", false));
        }
        else
        {
            negotiationText.Add(new Dialogue(DialogueType.Enemy, "Don't let me catch you sneaking around again.\n(Negotiation success)", false));
        }
        startDialogue();
    }

    void startDialogue()
    {
        nextButton.gameObject.SetActive(false);
        textCount = 0;
        StartCoroutine(ShowText(negotiationText[dialogueIndex]));

        var dialog = negotiationText[dialogueIndex];
        if (dialog.isDice && dialog.dicePoint != 0)
        {
            AudioManager.Instance.playDiceSound();
            diceView.SetActive(true);
            diceAnim.enabled = true;
            diceAnim.SetTrigger("IsRoll");
            StartCoroutine(Helper.Delay_RealTime(() => { diceAnim.SetTrigger("Is" + dialog.dicePoint); }, 1f));
        }
        else
        {
            diceView.SetActive(false);
        }
    }

    public void nextDialog()
    {
        dialogueIndex++;
        if (dialogueIndex < negotiationText.Count)
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
        negotiationText.Clear();
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

        for (int i = 0; i < dialog.text.Length; i++)
        {
            if (isSpeedUp)
            {
                dialogueText.text = dialog.text;
                if (dialog.isDice && dialog.dicePoint != 0)
                {
                    diceAnim.enabled = false;
                    dice.GetComponent<Image>().sprite = diceSprites[dialog.dicePoint - 1];
                }

                isSpeedUp = false;
                break;
            }

            if (dialog.text[i] == '<')
            {
                string tag = "";
                while (dialog.text[i] != '>')
                {
                    tag += dialog.text[i];
                    i++;
                }
                tag += dialog.text[i];
                dialogueText.text += tag;
            }
            else
            {
                dialogueText.text += dialog.text[i];
            }

            if (textCount % 3 == 0)
            {
                AudioManager.Instance.playDialogueSound();
            }
            textCount++;

            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        nextButton.gameObject.SetActive(true);
        nextButton.GetComponent<Button>().Select();
    }
}
