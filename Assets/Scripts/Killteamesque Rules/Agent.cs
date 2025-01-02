using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour, IDamagable, ISelectable
{
    public string Name;
    [SerializeField] LayerMask walkableLayer;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Actions[] availableActions;
    [SerializeField] DatasheetTemplate datasheetTemplate;
    [SerializeField] WeaponTemplate weaponTemplate;
    private Datasheet datasheet;
    public  Weapon HeldWeapons;
    public int Wounds => datasheet.Wounds;
    public int Defence { get => datasheet.Defence; }
    public int Save { get => datasheet.Save; }
    public int AP { get => datasheet.AP; }
    public int Movement { get => datasheet.Movement; }
    public List<IAction> Actions { get; private set; }
    public AgentController Controller { get; private set; }
    public bool Activated { get; set; }
    public bool IsDead => datasheet.Wounds > 0;

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        datasheet = datasheetTemplate.GenerateData();
        HeldWeapons = weaponTemplate.GenerateData();
        Controller = new(GetComponent<NavMeshAgent>(), Movement, walkableLayer);
        Actions = new();
        foreach (var action in availableActions)
        {
            switch (action)
            {
                case global::Actions.Move:
                    Actions.Add(new MoveAction(Controller));
                    break;
                case global::Actions.Attack:
                    Actions.Add(new ShootAction(HeldWeapons, targetLayer));
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
    public void ConsumeAP(int amount) => datasheet.ConsumeAP(amount);
    public void GenerateAP() => datasheet.ResetAP();
    public void DealDamage(int amount)
    {
        if (IsDead) return;

        ObjectPool.
            Instance
            .SpawnFromPool(
            "textPopup",
            transform.position,
            Quaternion.identity
            )
            .GetComponent<TextPopup>()
            .SetText(amount.ToString());
        datasheet.Wounds -= amount;
    }

    public void OnSelect()
    {
        throw new System.NotImplementedException();
    }

    public void OnDeselect()
    {
        throw new System.NotImplementedException();
    }
}

public enum Actions
{
    Move,
    Attack,
    Idle,
    None
}