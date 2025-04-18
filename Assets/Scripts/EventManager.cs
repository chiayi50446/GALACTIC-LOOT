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
    public event Action<int> RemoveInventory;
    public event Action<string, int> UpdateUserTakenItem;
    public event Action UpdateAlertnessLevel;
    public event Action UpdateVision;
    public event Action ActiveBoss;
    public event Action ActiveBossHealth;
    public event Action ActivePlayerHealth;
    public event Action ActiveNegotiationCheck;
    public event Action ClearLevel;
    public event Action BossDead;
    public event Action ShowCollectItem;
    public event Action<int> UpdateDisguiseCount;
    public event Action<GameObject> DisplayGuide;

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

    public void TriggerLoadingActive()
    {
        LoadingActiveEvent?.Invoke();
    }

    public void TriggerUpdateInventory(string itemName, int playerNum)
    {
        UpdateInventory?.Invoke(itemName, playerNum);
    }

    public void TriggerRemoveInventory(int playerNum)
    {
        RemoveInventory?.Invoke(playerNum);
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

    public void TriggerActiveBoss()
    {
        ActiveBoss?.Invoke();
    }

    public void TriggerActiveBossHealthUI()
    {
        ActiveBossHealth?.Invoke();
    }

    public void TriggerActivePlayerHealthUI()
    {
        ActivePlayerHealth?.Invoke();
    }

    public void TriggerActiveNegotiationCheck()
    {
        ActiveNegotiationCheck?.Invoke();
    }

    public void TriggerClearLevel()
    {
        ClearLevel?.Invoke();
    }

    public void TriggerBossDead()
    {
        BossDead?.Invoke();
    }

    public void TriggerShowCollectItem()
    {
        ShowCollectItem?.Invoke();
    }

    public void TriggerDisplayGuide(GameObject displayGuide)
    {
        DisplayGuide?.Invoke(displayGuide);
    }

    public void TriggerUpdateDisguiseCount(int pNum)
    {
        UpdateDisguiseCount?.Invoke(pNum);
    }
}
