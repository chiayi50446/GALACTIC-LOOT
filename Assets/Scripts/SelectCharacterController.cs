using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCharacterController : MenuController
{
    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private Sprite[] characterAva;
    [SerializeField]
    private GameObject Player1;
    [SerializeField]
    private GameObject Player2;
    private Transform Avatar;
    private Transform Attack;
    private Transform Health;
    private Transform Charisma;
    private Transform Load;
    private Transform Panel;
    private GameObject OKButton;
    private GameObject Title;
    private CharacterType CurrentType = CharacterType.type1;
    void Awake()
    {
        OKButton = transform.GetChild(0).Find("ButtonGroup/OK").gameObject;
        Title = transform.GetChild(0).Find("Title").gameObject;
        EventManager.Instance.SelectCharacterEvent += ChangeCharacterInfo;
    }
    void OnEnable()
    {
        Init();
    }

    void OnDestroy()
    {
        EventManager.Instance.SelectCharacterEvent -= ChangeCharacterInfo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MakeSureCharacter();
        }

        MoveSelection();
    }

    public void OK()
    {
        DataPersistentManager.instance.EntryGame();
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        startMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void Init()
    {
        GameState.Instance.SetPlayer1Type(CharacterType.none);
        GameState.Instance.SetPlayer2Type(CharacterType.none);

        OKButton.SetActive(false);
        startButton.GetComponent<Button>().Select();

        CurrentType = CharacterType.type1;

        SetCurrentSelectPlayer(Player2);
        SetCharacterData(CurrentType);
        SetCurrentSelectPlayer(Player1);
        SetCharacterData(CurrentType);

        Player1.transform.Find("Panel").gameObject.SetActive(false);
        Player2.transform.Find("Panel").gameObject.SetActive(true);
    }

    private void ChangeCharacterInfo(CharacterType type)
    {
        CurrentType = type;
        SetCharacterData(type);
    }

    public void MakeSureCharacter()
    {
        if (GameState.Instance.GetPlayerType(1) == CharacterType.none)
        {
            GameState.Instance.SetPlayer1Type(CurrentType);
            Panel.gameObject.SetActive(true); // player1 panel open
            SetCurrentSelectPlayer(Player2);
            Panel.gameObject.SetActive(false);// player2 panel close
            startButton.GetComponent<Button>().Select();
            CurrentType = CharacterType.type1;
            ChangeCharacterInfo(CharacterType.type1);
            Title.GetComponent<TMP_Text>().text = "Select Player 2";
        }
        else
        {
            Panel.gameObject.SetActive(true);// player2 panel open
            GameState.Instance.SetPlayer2Type(CurrentType);

            OKButton.SetActive(true);
            OKButton.GetComponent<Button>().Select();
            Title.GetComponent<TMP_Text>().text = "Complete Select";
        }
    }

    private void SetCharacterData(CharacterType type)
    {
        Avatar.gameObject.GetComponent<Image>().sprite = characterAva[(int)type - 1];
        var data = GameState.charactersData[type];
        Attack.gameObject.GetComponent<BarController>().SetValue(data.Attack);
        Health.gameObject.GetComponent<BarController>().SetValue(data.Health);
        Charisma.gameObject.GetComponent<BarController>().SetValue(data.Charisma);
        Load.gameObject.GetComponent<BarController>().SetValue(data.Load);
    }

    private void SetCurrentSelectPlayer(GameObject player)
    {
        Avatar = player.transform.Find("Avatar");
        Attack = player.transform.Find("Attack");
        Health = player.transform.Find("Health");
        Charisma = player.transform.Find("Charisma");
        Load = player.transform.Find("Load");
        Panel = player.transform.Find("Panel");
    }

    private void MoveSelection()
    {
        Selectable current = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();
        if (current != null)
        {
            Selectable next = null;
            if (Input.GetKeyDown(KeyCode.L)) next = current.FindSelectableOnRight();
            if (Input.GetKeyDown(KeyCode.J)) next = current.FindSelectableOnLeft();
            if (Input.GetKeyDown(KeyCode.I)) next = current.FindSelectableOnUp();
            if (Input.GetKeyDown(KeyCode.K)) next = current.FindSelectableOnDown();

            if (next != null)
            {
                EventSystem.current.SetSelectedGameObject(next.gameObject);
            }
        }
    }
}
