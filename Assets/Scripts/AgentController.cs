using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    [SerializeField] float sampleDistance = 0.5f;
    NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public bool TrySetDestination(Vector3 destination)
    {
        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, sampleDistance, agent.areaMask))
        {
            agent.SetDestination(hit.position);
            return true;
        }
        return false;
    }
}
