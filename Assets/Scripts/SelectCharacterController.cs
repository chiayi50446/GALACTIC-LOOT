using System;
using System.Collections.Generic;
using UnityEngine;
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
    private CharacterType CurrentType = CharacterType.type1;
    void Awake()
    {
        OKButton = transform.GetChild(0).Find("OK").gameObject;
        EventManager.instance.SelectCharacterEvent += ChangeCharacterInfo;
    }
    void OnEnable()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MakeSureCharacter();
        }
    }

    public void OK()
    {
        DataPersistentManager.instance.LoadGame();
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

        OKButton.GetComponent<Button>().interactable = false;
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
        if (GameState.Instance.GetPlayer1Type() == CharacterType.none)
        {
            GameState.Instance.SetPlayer1Type(CurrentType);
            Panel.gameObject.SetActive(true); // player1 panel open
            SetCurrentSelectPlayer(Player2);
            Panel.gameObject.SetActive(false);// player2 panel close
            startButton.GetComponent<Button>().Select();
            CurrentType = CharacterType.type1;
            ChangeCharacterInfo(CharacterType.type1);
        }
        else
        {
            GameState.Instance.SetPlayer2Type(CurrentType);
            OKButton.GetComponent<Button>().interactable = true;
            OKButton.GetComponent<Button>().Select();
        }
    }

    private void SetCharacterData(CharacterType type)
    {
        Avatar.gameObject.GetComponent<Image>().sprite = characterAva[(int)type - 1];
        var data = GameState.characters[type];
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
}
