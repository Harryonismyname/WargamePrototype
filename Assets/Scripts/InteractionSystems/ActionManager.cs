using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolearmStudios.Utils;

public class ActionManager : MonoBehaviour
{
    public string CurrentActionName;
    IAction currentAction;
    StateMachine<TurnState> turnStateMachine;
    Agent currentAgent;

    private void Awake()
    {
        PlayerInputHandler.OnClick += HandleClick;
        PlayerAgentManager.OnAgentSelected += UpdateCurrentAgent;
    }

    private void Start() => turnStateMachine = TurnManager.StateMachine;

    private void OnDestroy()
    {
        PlayerInputHandler.OnClick -= HandleClick;
        PlayerAgentManager.OnAgentSelected -= UpdateCurrentAgent;
    }

    private void UpdateCurrentAgent(Agent obj)
    {
        if (obj == null) return;
        if (obj == currentAgent) return;
        currentAgent = obj;
        SelectAction(currentAgent.Actions[0]);
    }

    private void HandleClick(Vector3 point)
    {
        if (turnStateMachine.State != TurnState.Declaration) return;
        if (currentAction == null) return;

        if (currentAgent.AP - currentAction.Cost < 0) return;
        if (!DeclareAction(point)) return;

        currentAgent.ConsumeAP(currentAction.Cost);
        if (currentAgent.AP == 0)
        {
            turnStateMachine.ChangeState(TurnState.Resolution);
        }
    }

    public void SelectAction(IAction newAction)
    {
        if (newAction == null) return;
        if (newAction == currentAction) return;
        currentAction = newAction;
        CurrentActionName = currentAction.Name;
    }

    public void SelectAction(int index) => SelectAction(currentAgent.Actions[Mathf.Clamp(index, 0, currentAgent.Actions.Count)]);

    public bool DeclareAction(Vector3 point) => currentAction.Declare(point);

    public bool ConfirmAction(Vector3 point) => currentAction.Perform(point);

    public bool CancelAction() => currentAction.Cancel();
}

public enum ActionState
{
    Idle,
    Declared,
    Performing,
    Complete,
    Canceling,
    Canceled
}