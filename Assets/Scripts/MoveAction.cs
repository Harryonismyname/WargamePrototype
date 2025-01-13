using PolearmStudios.Utils;
using System;
using UnityEngine;
public class MoveAction : IAction
{
    public string Name { get => "Move"; }

    private readonly int cost = 1;

    private readonly AgentController controller;

    public MoveAction(AgentController _controller)
    {
        controller = _controller;
        StateMachine = new();
    }

    public int Cost => cost;

    public StateMachine<ActionState> StateMachine {get; private set;}

    public bool Declare(Vector3 point)
    {
        return Perform(controller.CalculatePath(point));
    }

    public bool Perform(Vector3 point)
    {
        if (!controller.TrySetDestination(point))
        {
            Debug.LogWarning("Path Not Found!");
            return false;
        }
        return true;
    }

    private bool Complete()
    {
        StateMachine.ChangeState(ActionState.Complete);
        // wrap up any additional things before completion
        return true;
    }

    public bool Cancel()
    {
        StateMachine.ChangeState(ActionState.Canceling);
        // do stuff to cancel this action...
        return true;
    }
}