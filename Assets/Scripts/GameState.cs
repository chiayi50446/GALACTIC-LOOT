using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    private static GameState instance = null;
    [SerializeField]
    private int currentLevel;

    public static Dictionary<CharacterType, CharacterProperty> characters = new Dictionary<CharacterType, CharacterProperty>
    {
        {CharacterType.type1, new CharacterProperty(){Attack = 2, Health=3, Communicate =0, CarryCapacity=3 }},
        {CharacterType.type2, new CharacterProperty(){Attack = 1, Health=5, Communicate =0, CarryCapacity=3 }},
        {CharacterType.type3, new CharacterProperty(){Attack = 1, Health=3, Communicate =1, CarryCapacity=3 }},
        {CharacterType.type4, new CharacterProperty(){Attack = 1, Health=3, Communicate =0, CarryCapacity=5 }}
    };

    private GameState()
    {
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
}

public class CharacterProperty
{
    public int Attack { get; set; }
    public int Health { get; set; }
    public int Communicate { get; set; }
    public int CarryCapacity { get; set; }
}

public enum CharacterType
{
    type1,
    type2,
    type3,
    type4
}