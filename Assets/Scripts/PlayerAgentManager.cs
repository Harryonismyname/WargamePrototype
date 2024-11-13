using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgentManager : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] Agent[] availableAgents;
    public Agent ActiveAgent { get; private set; }

    IAction currentAction;
    
    int index = 0;

    private void Awake()
    {
        PlayerInputHandler.OnClick += AttemptAction;
    }
    private void OnDestroy()
    {
        PlayerInputHandler.OnClick -= AttemptAction;
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

    public void SelectAction(IAction newAction)
    {
        currentAction = newAction;
    }

    void AttemptAction(Vector3 point)
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
        if (index >= availableAgents.Length) index = 0;
        ActiveAgent = availableAgents[index];
    }
}
