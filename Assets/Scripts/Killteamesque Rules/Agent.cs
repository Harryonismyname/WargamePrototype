using System.Collections.Generic;
using UnityEngine;

public class Agent: MonoBehaviour
{
    public string Name;
    [SerializeField] int APL = 2;
    public int Defence { get; private set; }
    public int Save {  get; private set; }
    public int AP { get; private set; }
    [SerializeField] AgentController controller;
    [SerializeField] Actions[] availableActions;
    public List<IAction> Actions { get; private set; }

    private void Awake()
    {
        controller = GetComponent<AgentController>();
        Actions = new();
        foreach (var action in availableActions)
        {
            switch (action)
            {
                case global::Actions.Move:
                    Actions.Add(new MoveAction(controller));
                    break;
                case global::Actions.Attack:
                    break;
                case global::Actions.Idle:
                    break;
                case global::Actions.None:
                    break;
                default:
                    break;
            }
        }
    }
    public void ConsumeAP(int amount)
    {
        AP = Mathf.Clamp(AP - amount, 0, APL);
    }
    public void GenerateAP()
    {
        AP = APL;
    }
}

public enum Actions
{
    Move,
    Attack,
    Idle,
    None
}