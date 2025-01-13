using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolearmStudios.Utils;
using System;
public class ShootAction : IAction
{
    public string Name { get => "Shoot"; }
    public int Cost => cost;
    private readonly int cost = 1;
    private readonly Weapon weapon;
    private readonly LayerMask layerMask;
    private readonly Collider[] targetCollider = new Collider[1];
    private Transform agent;
    private Agent target;

    public StateMachine<ActionState> StateMachine { get; private set; }

    private bool CheckForTarget()
    {
        if (!CheckClick()) return false;
        agent = targetCollider[0].transform;
        var localTargets = Physics.OverlapSphere(agent.position, weapon.WeaponRange, layerMask);
        return true;
    }

    private bool CheckClick()
    {
        if (!Utilities.ScreenToWorldPoint(out Vector3 hit, layerMask)) return false;
        Physics.OverlapSphereNonAlloc(hit, 2, targetCollider, layerMask);
        if (!targetCollider[0].TryGetComponent(out target)) return false;
        return true;
    }
    private CombatContext RollForAttack(CombatContext context)
    {
        Debug.Log("Rolling Attack Dice...");
        int result;
        for (int i = 0; i < weapon.WeaponAttack; i++)
        {
            result = DiceBox.RollD6();
            if (result < weapon.WeaponSkill) continue;
            if (result == 6)
            {
                context.RetainedCriticalHits.Add(result);
                continue;
            }
            context.RetainedHits.Add(result);
        }
        Debug.Log($"Retained {context.RetainedHits.Count + context.RetainedCriticalHits.Count} Hits!");
        return context;
    }
    private CombatContext RollForDefense(CombatContext context)
    {
        Debug.Log("Rolling Defense Dice...");
        int result;
        for (int i = 0; i < target.Defence; i++)
        {
            result = DiceBox.RollD6();
            if (result < target.Save) continue;
            if (result == 6)
            {
                context.RetainedCriticalDefense.Add(result);
                continue;
            }
            context.RetainedDefense.Add(result);
        }
        Debug.Log($"Retained {context.RetainedDefense.Count + context.RetainedCriticalDefense.Count} Blocks!");
        return context;
    }

    public ShootAction(Weapon _weapon, LayerMask _targetMask)
    {
        weapon = _weapon;
        layerMask = _targetMask;
        StateMachine = new();
    }

    public bool Declare(Vector3 point)
    {
        if (!CheckForTarget())
        {
            StateMachine.ChangeState(ActionState.Canceled);
            return false;
        }
        return Perform(point);
    }

    public bool Perform(Vector3 point)
    {
        Debug.Log("Performing Shoot Action...");
        CombatContext context = new(weapon, target);
        context = RollForDefense(RollForAttack(context));
        context.DetermineSuccessfulHits();
        int totalDamage = 0;
        foreach (var _ in context.SuccessfulHits)
        {
            target.DealDamage(weapon.WeaponDamage);
            totalDamage += weapon.WeaponDamage;
        }
        foreach (var _ in context.SuccessfulCrits)
        {
            target.DealDamage(weapon.WeaponDamage * 2);
            totalDamage += weapon.WeaponDamage;
        }
        Debug.Log($"Dealing {totalDamage} total damage!");
        return true;
    }

    private bool Complete()
    {
        StateMachine.ChangeState(ActionState.Complete);
        // wrap up any additional things before completion
        return true;
    }
    public bool Cancel()
    {
        StateMachine.ChangeState(ActionState.Canceling);
        // do stuff to cancel this action...
        return true;
    }
}
