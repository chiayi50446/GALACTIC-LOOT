using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    private static GameState instance = null;
    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private CharacterType Player1Type = CharacterType.none;
    [SerializeField]
    private CharacterType Player2Type = CharacterType.none;
    [SerializeField]
    private int Player1ItemLoadIndex = -1;
    [SerializeField]
    private int Player2ItemLoadIndex = -1;
    [SerializeField]
    private int Player1SelectItemIndex = 0;
    [SerializeField]
    private int Player2SelectItemIndex = 0;
    private bool IsCollectItemGet = false;
    [SerializeField] private static float currentAlertnessLevel;
    public int[] BossHealth = new[] { 5, 10 };

    public static Dictionary<CharacterType, CharacterProperty> charactersData = new Dictionary<CharacterType, CharacterProperty>
    {
        {CharacterType.type1, new CharacterProperty(){Attack = 2, Health=3, Charisma =0, Load=1 }},
        {CharacterType.type2, new CharacterProperty(){Attack = 1, Health=5, Charisma =0, Load=1 }},
        {CharacterType.type3, new CharacterProperty(){Attack = 1, Health=3, Charisma =1, Load=1 }},
        {CharacterType.type4, new CharacterProperty(){Attack = 1, Health=3, Charisma =0, Load=3 }}
    };
    public static Dictionary<CharacterType, RuntimeAnimatorController> animatorController;
    public static Dictionary<CharacterType, Sprite> characterAvatar;
    public static Dictionary<string, Sprite> chestItem;

    private GameState()
    {
        animatorController = new Dictionary<CharacterType, RuntimeAnimatorController>(){
            {CharacterType.type1, (RuntimeAnimatorController)Resources.Load("Animations/PlayerAnimator_1", typeof(RuntimeAnimatorController ))},
            {CharacterType.type2, (RuntimeAnimatorController)Resources.Load("Animations/PlayerAnimator_2", typeof(RuntimeAnimatorController ))},
            {CharacterType.type3, (RuntimeAnimatorController)Resources.Load("Animations/PlayerAnimator_3", typeof(RuntimeAnimatorController ))},
            {CharacterType.type4, (RuntimeAnimatorController)Resources.Load("Animations/PlayerAnimator_4", typeof(RuntimeAnimatorController ))}
        };
        characterAvatar = new Dictionary<CharacterType, Sprite>()
        {
            {CharacterType.type1, (Sprite)Resources.Load("Avatars/ava_1", typeof(Sprite))},
            {CharacterType.type2, (Sprite)Resources.Load("Avatars/ava_2", typeof(Sprite))},
            {CharacterType.type3, (Sprite)Resources.Load("Avatars/ava_3", typeof(Sprite))},
            {CharacterType.type4, (Sprite)Resources.Load("Avatars/ava_4", typeof(Sprite))}
        };
        chestItem = new Dictionary<string, Sprite>()
        {
            {"Bomb", (Sprite)Resources.Load("ChestItems/Bomb", typeof(Sprite))},
            {"Rifle", (Sprite)Resources.Load("ChestItems/Rifle", typeof(Sprite))},
            {"WizardHat", (Sprite)Resources.Load("ChestItems/WizardHat", typeof(Sprite))}
        };
        currentLevel = 1;
        currentAlertnessLevel = 0;
    }

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameState();
            }
            return instance;
        }
    }

    public void CreateNewGame()
    {
        instance = new GameState();
    }

    public void SetGameState(GameState gameState)
    {
        instance = gameState;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public string GetCurrentLoadScene()
    {
        return currentLevel == 1 ? "Level1Scene" : "Level2Scene";
    }

    public void SetPlayer1Type(CharacterType type)
    {
        Player1Type = type;
    }
    // public CharacterType GetPlayer1Type()
    // {
    //     return Player1Type;
    // }
    public CharacterType GetPlayerType(int pNum)
    {
        if (pNum == 1)
            return Player1Type;
        else
            return Player2Type;
    }
    public void SetPlayer2Type(CharacterType type)
    {
        Player2Type = type;
    }
    // public CharacterType GetPlayer2Type()
    // {
    //     return Player2Type;
    // }
    public bool IsPlayerItemFull(int playerNum)
    {
        if (playerNum == 1)
        {
            var play1MaxLoad = charactersData[Player1Type].Load;
            return Player1ItemLoadIndex == play1MaxLoad - 1;
        }
        else
        {
            var play2MaxLoad = charactersData[Player2Type].Load;
            return Player2ItemLoadIndex == play2MaxLoad - 1;
        }
    }
    public int GetPlayerItemLoad(int playerNum)
    {
        if (playerNum == 1)
        {
            var play1MaxLoad = charactersData[Player1Type].Load;
            if (Player1ItemLoadIndex < play1MaxLoad - 1)
            {
                Player1ItemLoadIndex++;
            }
            return Player1ItemLoadIndex;
        }
        else
        {
            var play2MaxLoad = charactersData[Player2Type].Load;
            if (Player2ItemLoadIndex < play2MaxLoad - 1)
            {
                Player2ItemLoadIndex++;
            }
            return Player2ItemLoadIndex;
        }
    }
    public void ReducePlayerItemLoad(int playerNum)
    {
        if (playerNum == 1)
        {
            Player1ItemLoadIndex--;
        }
        else
        {
            Player2ItemLoadIndex--;
        }
    }
    public void SetPlayerSelectItem(int playerNum, int index)
    {
        if (playerNum == 1)
        {
            Player1SelectItemIndex = index;
        }
        else
        {
            Player1SelectItemIndex = index;
        }
    }
    public int GetPlayerSelectItem(int playerNum)
    {
        if (playerNum == 1)
        {
            return Player1SelectItemIndex;
        }
        else
        {
            return Player2SelectItemIndex;
        }
    }

    public void SetIsCollectItemGet()
    {
        IsCollectItemGet = true;
    }
    public bool GetIsCollectItemGet()
    {
        return IsCollectItemGet;
    }

    public void SetAlertnessLevel(float newAlertnessLevel)
    {
        currentAlertnessLevel = newAlertnessLevel;
    }

    public float GetAlertnessLevel()
    {
        return currentAlertnessLevel;
    }
}

public class CharacterProperty
{
    public int Attack { get; set; }
    public int Health { get; set; }
    public int Charisma { get; set; }
    public int Load { get; set; }
}

public enum CharacterType
{
    none = 0,
    type1 = 1,
    type2 = 2,
    type3 = 3,
    type4 = 4
}