using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgentNameslate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI slate;
    [SerializeField] TextMeshProUGUI apDisplay;
    PlayerAgentManager manager;
    private void Awake()
    {
        manager = FindFirstObjectByType<PlayerAgentManager>();
    }

    private void Update()
    {
        slate.text = manager.ActiveAgent.Name;
        apDisplay.text = manager.ActiveAgent.AP.ToString();
    }
}
