using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolearmStudios.Utils;

public class TurnManager : MonoBehaviour
{
    public static Singleton<TurnManager> Instance;
    public static StateMachine<TurnState> StateMachine;

    private void Awake() => Initialize();
    
    private void Start() => UpdateState(TurnState.Selection);

    private void Initialize()
    {
        StateMachine = new StateMachine<TurnState>();
        Instance = new(this);
    }

    public void UpdateState(TurnState newState) => StateMachine.ChangeState(newState);

}

public enum TurnState
{
    Selection,
    Declaration,
    Reaction,
    Resolution,
    None
}