using UnityEngine;

public class MoveAction : IAction
{
    public readonly string Name = "Move";

    private readonly int cost = 1;
    private readonly int maxRange = 20;

    private readonly AgentController controller;
    public MoveAction(AgentController _controller)
    {
        controller = _controller;
    }

    public int Cost => cost;

    public bool Declare(Vector3 point)
    {
        return Perform(point);
    }

    public bool Perform(Vector3 point)
    {
        if (Vector3.Distance(point, controller.transform.position) >= maxRange)
        {
            Vector3 dir = Vector3.ClampMagnitude(point - controller.transform.position, maxRange);
            point = Camera.main.WorldToScreenPoint( dir + controller.transform.position);
            Debug.DrawRay(Camera.main.ScreenPointToRay(point).origin, Camera.main.ScreenPointToRay(point).direction, Color.red, Mathf.Infinity);
            Debug.DrawLine(Camera.main.ScreenPointToRay(point).origin, dir + controller.transform.position, Color.green, Mathf.Infinity);
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(point),out RaycastHit hit,  Mathf.Infinity, default))
            {
                Debug.LogWarning("Cast Failed!");
                DebugFailure(dir + controller.transform.position);
                return false;
            }
            point = hit.point;
        }
        if (!controller.TrySetDestination(point))
        {
            DebugFailure(point);
            Debug.LogWarning("Path Not Found!");
            return false;
        }
        return true;
    }

    private void DebugFailure(Vector3 point)
    {
        var errorBubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        errorBubble.transform.position = point;
        errorBubble.GetComponent<MeshRenderer>().material.color = Color.red;
    }
}