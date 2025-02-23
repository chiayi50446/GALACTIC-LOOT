using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action LoadingActiveEvent;
    public event Action<CharacterType> SelectCharacterEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void TriggerLoadingActive()
    {
        LoadingActiveEvent?.Invoke();
    }

    public void SelectCharacterActive(CharacterType type)
    {
        SelectCharacterEvent?.Invoke(type);
    }
}
