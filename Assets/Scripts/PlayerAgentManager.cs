using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PolearmStudios.SelectionSystem;

public class PlayerAgentManager : MonoBehaviour
{
    [SerializeField] LayerMask AgentLayer;
    [SerializeField] List<Agent> availableAgents;
    public Agent ActiveAgent { get; private set; }
    public static event Action<Agent> OnAgentSelected;
    
    int index = 0;

    private void Awake()
    {
        TurnManager.StateMachine.OnStateChanged += HandleState;
        Selector.OnSelectionUpdated += ValidateSelection;
    }

    private void OnDestroy()
    {
        TurnManager.StateMachine.OnStateChanged -= HandleState;
        Selector.OnSelectionUpdated -= ValidateSelection;
    }

    private void Start()
    {
        ActiveAgent = availableAgents[index];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CycleActiveAgent();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectAgent();
        }
    }

    private void HandleState(TurnState obj)
    {
        switch (obj)
        {
            case TurnState.Selection:
                break;
            case TurnState.Declaration:
                break;
            case TurnState.Reaction:
                break;
            case TurnState.Resolution:
                break;
            case TurnState.None:
                break;
            default:
                break;
        }
    }

    private void ValidateSelection(ISelectable obj)
    {
        if (obj == null) return;
        if (TurnManager.StateMachine.State != TurnState.Selection) return;
        if (!obj.GameObject.TryGetComponent(out Agent newAgent)) return;
        if (!availableAgents.Contains(newAgent)) return;

        index = availableAgents.IndexOf(newAgent);
        ActiveAgent = newAgent;
    }

    private void SelectAgent()
    {
        OnAgentSelected?.Invoke(ActiveAgent);
        ActiveAgent.GenerateAP();
        TurnManager.StateMachine.ChangeState(TurnState.Declaration);
    }

    private void CycleActiveAgent()
    {
        if (TurnManager.StateMachine.State != TurnState.Selection)
        {
            Debug.LogWarning("Once an agent is selected you cannot choose another");
            return;
        }
        index++;
        if (index >= availableAgents.Count) index = 0;
        ActiveAgent = availableAgents[index];
    }
}
