using UnityEngine;
public class MoveAction : IAction
{
    public string Name { get => "Move"; }

    private readonly int cost = 1;

    private readonly AgentController controller;
    public MoveAction(AgentController _controller)
    {
        controller = _controller;
    }

    public int Cost => cost;

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
}