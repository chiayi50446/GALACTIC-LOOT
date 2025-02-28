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

    public static Dictionary<CharacterType, CharacterProperty> charactersData = new Dictionary<CharacterType, CharacterProperty>
    {
        {CharacterType.type1, new CharacterProperty(){Attack = 2, Health=3, Charisma =0, Load=1 }},
        {CharacterType.type2, new CharacterProperty(){Attack = 1, Health=5, Charisma =0, Load=1 }},
        {CharacterType.type3, new CharacterProperty(){Attack = 1, Health=3, Charisma =1, Load=1 }},
        {CharacterType.type4, new CharacterProperty(){Attack = 1, Health=3, Charisma =0, Load=3 }}
    };
    public static Dictionary<CharacterType, RuntimeAnimatorController> animatorController;
    public static Dictionary<CharacterType, Sprite> characterAvatar;

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
        currentLevel = 1;
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
    public CharacterType GetPlayer1Type()
    {
        return Player1Type;
    }

    public void SetPlayer2Type(CharacterType type)
    {
        Player2Type = type;
    }
    public CharacterType GetPlayer2Type()
    {
        return Player2Type;
    }
}

public class CharacterProperty
{
    public float Attack { get; set; }
    public float Health { get; set; }
    public float Charisma { get; set; }
    public float Load { get; set; }
}

public enum CharacterType
{
    none = 0,
    type1 = 1,
    type2 = 2,
    type3 = 3,
    type4 = 4
}