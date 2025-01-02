using PolearmStudios.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingVisual : MonoBehaviour
{
    [SerializeField] PlayerAgentManager agentManager;
    [SerializeField] LayerMask targetMask;
    AgentController controller;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (TurnManager.StateMachine.State != TurnState.Declaration) return;
        controller = agentManager.ActiveAgent.Controller;
        if (Utilities.ScreenToWorldPoint(out Vector3 point, targetMask))
        {
            Vector3 end = controller.CalculatePath(point);
            if (end == agentManager.ActiveAgent.transform.position) return;
            if (end != controller.Path.corners[^1]) controller.CalculatePath(end);
            if (controller.Path.corners.Length < 1) return;
            lineRenderer.positionCount = controller.Path.corners.Length;
            for (int i = 0; i < controller.Path.corners.Length; i++) 
            {
                var corner = controller.Path.corners[i];
                lineRenderer.SetPosition(i, corner);
            }
        }
    }
}
