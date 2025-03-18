using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    private static GameState instance = null;
    [SerializeField]
    private Level currentLevel;
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
    private Dictionary<Level, bool> IsCollectItemGet;
    [SerializeField] private static float currentAlertnessLevel;
    public Dictionary<Level, int> BossHealth = new Dictionary<Level, int>
    {
        {Level.Level1, 5},
        {Level.Level2, 10},
    };

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
    public static Dictionary<EnemyType, Sprite> enemyAvatar;
    public EnemyType lastAlertEnemyType;
    public CharacterType lastAlertPlayerType;
    public Dictionary<Level, bool> isLevelClear;
    public static Dictionary<Level, float> clearTargetTime = new Dictionary<Level, float>(){
        {Level.Level1, 120},
        {Level.Level2, 300},
    };
    private Dictionary<Level, float> clearLevelTime;
    private Dictionary<Level, int> playerDeathNum;

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
        enemyAvatar = new Dictionary<EnemyType, Sprite>()
        {
            {EnemyType.stationary, (Sprite)Resources.Load("Avatars/guard_1", typeof(Sprite))},
            {EnemyType.patrol, (Sprite)Resources.Load("Avatars/guard_2", typeof(Sprite))}
        };
        chestItem = new Dictionary<string, Sprite>()
        {
            {"Bomb", (Sprite)Resources.Load("ChestItems/Bomb", typeof(Sprite))},
            {"Rifle", (Sprite)Resources.Load("ChestItems/Rifle", typeof(Sprite))},
            {"WizardHat", (Sprite)Resources.Load("ChestItems/WizardHat", typeof(Sprite))}
        };
        IsCollectItemGet = new Dictionary<Level, bool>()
        {
            {Level.Level1, false},
            {Level.Level2, false}
        };
        currentLevel = Level.Level1;
        currentAlertnessLevel = 0;
        isLevelClear = new Dictionary<Level, bool>(){
            {Level.Level1, false},
            {Level.Level2, false}
        };
        clearLevelTime = new Dictionary<Level, float>(){
            {Level.Level1, 0},
            {Level.Level2, 0}
        };
        playerDeathNum = new Dictionary<Level, int>()
        {
            {Level.Level1, 0},
            {Level.Level2, 0},
        };
        Player1ItemLoadIndex = -1;
        Player2ItemLoadIndex = -1;
        Player1SelectItemIndex = 0;
        Player2SelectItemIndex = 0;
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

    public void SetCurrentLevel(Level level)
    {
        currentLevel = level;
    }
    public Level GetCurrentLevel()
    {
        return currentLevel;
    }
    public string GetCurrentLoadScene()
    {
        return currentLevel == Level.Level1 ? "Level1Scene" : "Level2Scene";
    }

    public CharacterType GetPlayerType(int pNum)
    {
        if (pNum == 1)
            return Player1Type;
        else
            return Player2Type;
    }

    public void SetPlayer1Type(CharacterType type)
    {
        Player1Type = type;
    }

    public void SetPlayer2Type(CharacterType type)
    {
        Player2Type = type;
    }

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

    public int AddPlayerItemLoad(int playerNum)
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
    public int GetPlayerItemLoad(int playerNum)
    {
        if (playerNum == 1)
        {
            return Player1ItemLoadIndex;
        }
        else
        {
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

    public void SetIsCollectItemGet(Level level)
    {
        IsCollectItemGet[level] = true;
    }
    public bool GetIsCollectItemGet(Level level)
    {
        return IsCollectItemGet[level];
    }

    public void SetAlertnessLevel(float newAlertnessLevel)
    {
        newAlertnessLevel = (newAlertnessLevel > 3) ? 3 : newAlertnessLevel;
        currentAlertnessLevel = newAlertnessLevel;
    }

    public float GetAlertnessLevel()
    {
        return currentAlertnessLevel;
    }

    public void SetLastAlertEnemyType(EnemyType type)
    {
        lastAlertEnemyType = type;
    }

    public EnemyType GetLastAlertEnemyType()
    {
        return lastAlertEnemyType;
    }

    public void SetLastAlertPlayerType(int playerNum)
    {
        lastAlertPlayerType = GetPlayerType(playerNum);
    }

    public CharacterType GetLastAlertPlayerType()
    {
        return lastAlertPlayerType;
    }

    public void SetIsLevelClear(Level level, bool isClear)
    {
        isLevelClear[level] = isClear;
    }

    public bool GetIsLevelClear(Level level)
    {
        return isLevelClear[level];
    }

    public void SetClearLevelTime(Level level, float usedTime)
    {
        clearLevelTime[level] = usedTime;
    }

    public float GetClearLevelTime(Level level)
    {
        return clearLevelTime[level];
    }

    public void SetPlayerDeathNum(Level level)
    {
        playerDeathNum[level]++;
    }

    public int GetPlayerDeathNum(Level level)
    {
        return playerDeathNum[level];
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

public enum EnemyType
{
    stationary = 1,
    patrol = 2,
}

public enum Level
{
    Level1 = 1,
    Level2 = 2
}