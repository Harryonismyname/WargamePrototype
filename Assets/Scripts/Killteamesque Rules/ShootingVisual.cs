using PolearmStudios.Utils;
using UnityEngine;

public class ShootingVisual : MonoBehaviour
{
    [SerializeField] PlayerAgentManager agentManager;
    [SerializeField] LayerMask targetMask;
    int range;
    LineRenderer lineRenderer;
    Ray lineOfSight;

    private void Awake() => lineRenderer = GetComponent<LineRenderer>();
    private void Start() => lineRenderer.positionCount = 2;

    private void Update()
    {
        if (TurnManager.StateMachine.State != TurnState.Declaration) return;
        range = agentManager.ActiveAgent.HeldWeapons.WeaponRange;
        if (Utilities.ScreenToWorldPoint(out Vector3 point, targetMask))
        {
            lineOfSight.origin = agentManager.ActiveAgent.transform.position;
            lineOfSight.direction = (point - agentManager.ActiveAgent.transform.position).normalized;
            lineRenderer.SetPosition(0, lineOfSight.origin);
            lineRenderer.SetPosition(1, lineOfSight.GetPoint(range));
        }
    }
}
