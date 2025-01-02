using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolearmStudios.SelectionSystem;

public class PlayerAgentManager : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] LayerMask AgentLayer;
    [SerializeField] List<Agent> availableAgents;
    public Agent ActiveAgent { get; private set; }

    IAction currentAction;
    
    int index = 0;
    int sampleDistance = 2;

    public string CurrentActionName;
    Collider[] selectionColls = new Collider[1];

    private void Awake()
    {
        PlayerInputHandler.OnClick += HandleClick;
        TurnManager.StateMachine.OnStateChanged += HandleState;
        Selector.OnSelectionUpdated += ValidateSelection;
    }

    private void OnDestroy()
    {
        PlayerInputHandler.OnClick -= HandleClick;
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

    public void SelectAction(IAction newAction)
    {
        currentAction = newAction;
        CurrentActionName = currentAction.Name;
    }

    public void SelectAction(int index)
    {
        Debug.Log("Attempting to select action of index: " + index + "...");
        if (index >= ActiveAgent.Actions.Count)
        {
            Debug.LogError("Attempt Failed!");
            return;
        }
        currentAction = ActiveAgent.Actions[index];
        CurrentActionName = currentAction.Name;
    }

    private void HandleClick(Vector3 point)
    {
        switch (TurnManager.StateMachine.State)
        {
            case TurnState.Selection:
                break;
            case TurnState.Declaration:
                AttemptAction(point);
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
        if (!obj.GameObject.TryGetComponent(out Agent newAgent)) return;
        if (!availableAgents.Contains(newAgent)) return;

        index = availableAgents.IndexOf(newAgent);
        ActiveAgent = newAgent;
    }

    private void AttemptAction(Vector3 point)
    {
        if (TurnManager.StateMachine.State != TurnState.Declaration)
        {
            Debug.LogWarning("Cannot Perform action when not inside Declaration Step");
            return;
        }
        if (currentAction == null)
        {
            Debug.LogWarning("No Action Selected!");
            return;
        }
        if (ActiveAgent.AP - currentAction.Cost < 0)
        {
            Debug.LogWarning("Not enough APL");
            return;
        }
        if (!currentAction.Declare(point))
        {
            Debug.LogWarning("Action Failed");
            return;
        }
        ActiveAgent.ConsumeAP(currentAction.Cost);
        if (ActiveAgent.AP == 0)
        {
            TurnManager.StateMachine.ChangeState(TurnState.Resolution);
        }
    }

    private void SelectAgent()
    {
        SelectAction(ActiveAgent.Actions[0]);
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
