using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    [SerializeField] float sampleDistance = 0.5f;
    NavMeshAgent agent;
    bool debug;
    NavMeshPath path;

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
    public void Debug()
    {
        debug = true;
    }
    public void StopDebug()
    {
        debug = false;
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + (transform.up * agent.height), agent.radius * 1.5f);
        }
    }
}
