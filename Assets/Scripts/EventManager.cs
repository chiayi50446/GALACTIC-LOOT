using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public event Action LoadingActiveEvent;
    public event Action<CharacterType> SelectCharacterEvent;
    public event Action<string, int> UpdateInventory;
    public event Action<string, int> UpdateUserTakenItem;
    public event Action UpdateAlertnessLevel;
    public event Action UpdateVision;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void TriggerLoadingActive()
    {
        LoadingActiveEvent?.Invoke();
    }

    public void TriggerUpdateInventory(string itemName, int playerNum)
    {
        UpdateInventory?.Invoke(itemName, playerNum);
    }

    public void TriggerUpdateUserTakenItem(string itemName, int playerNum)
    {
        UpdateUserTakenItem?.Invoke(itemName, playerNum);
    }

    public void SelectCharacterActive(CharacterType type)
    {
        SelectCharacterEvent?.Invoke(type);
    }

    public void TriggerUpdateAlertnessLevel()
    {
        UpdateAlertnessLevel?.Invoke();
    }

    public void TriggerUpdateVision()
    {
        UpdateVision?.Invoke();
    }
}
