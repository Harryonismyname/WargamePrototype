using UnityEngine;
using UnityEngine.AI;

public class AgentController
{
    public LayerMask WalkableLayer { get; private set; }
    public int Range {  get; private set; }
    public NavMeshPath Path { get; private set; }
    readonly NavMeshAgent agent;
    readonly float sampleDistance = 10f;

    public AgentController(NavMeshAgent agent, int range, LayerMask walkableLayer)
    {
        this.agent = agent;
        Range = range;
        WalkableLayer = walkableLayer;
        Path = new NavMeshPath();
    }
    public Vector3 CalculatePath(Vector3 point)
    {
        if (!agent.CalculatePath(point, Path))
        {
            Debug.LogWarning("Path Not calculated");
            return agent.transform.position;
        }
        float distTraveled = Vector3.Distance(Path.corners[0], agent.transform.position);
        float dist = Vector3.Distance(Path.corners[0], agent.transform.position);
        for (int i = 0; i < Path.corners.Length; i++)
        {
            var dest = Path.corners[i];

            if (i > 0)
            {
                dist = Vector3.Distance(dest, Path.corners[i - 1]);
                distTraveled += dist;
            }
            if (distTraveled >= Range)
            {
                var ray = new Ray(Path.corners[i - 1], (dest - Path.corners[i - 1]).normalized).GetPoint(dist - (distTraveled - Range));
                agent.CalculatePath(ray, Path);
                break;
            }
        }
        return Path.corners[^1];
    }
    public bool TrySetDestination(Vector3 destination) => NavMesh.SamplePosition(destination, out NavMeshHit hit, sampleDistance, agent.areaMask) && agent.SetDestination(hit.position);
}
