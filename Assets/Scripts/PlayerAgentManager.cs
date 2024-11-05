using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgentManager : MonoBehaviour
{
    [SerializeField] AgentController[] availableAgents;
    AgentController activeAgent;
    int index = 0;

    private void Awake()
    {
        PlayerInputHandler.OnClick += MoveSelectedAgent;
    }
    private void OnDestroy()
    {
        PlayerInputHandler.OnClick -= MoveSelectedAgent;
    }

    private void Start()
    {
        activeAgent = availableAgents[index];
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            CycleActiveAgent();
        }
    }

    private void CycleActiveAgent()
    {
        index++;
        if (index >= availableAgents.Length) index = 0;
        activeAgent = availableAgents[index];
    }

    private void MoveSelectedAgent(Vector3 destination)
    {
        if (activeAgent.TrySetDestination(destination))
        {
            return;
        }
    }
}
