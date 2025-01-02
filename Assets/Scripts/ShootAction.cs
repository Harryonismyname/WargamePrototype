using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolearmStudios.Utils;
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

    private bool CheckForTarget()
    {
        if (!CheckClick()) return false;
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
        int result;
        for (int i = 0; i < weapon.WeaponAttack; i++)
        {
            result = Random.Range(1, 6);
            if (result < weapon.WeaponSkill) continue;
            if (result == 6)
            {
                context.RetainedCriticalHits.Add(result);
                continue;
            }
            context.RetainedHits.Add(result);
        }
        return context;
    }
    private CombatContext RollForDefense(CombatContext context)
    {
        int result;
        for (int i = 0; i < target.Defence; i++)
        {
            result = Random.Range(1, 6);
            if (result < target.Save) continue;
            if (result == 6)
            {
                context.RetainedCriticalDefense.Add(result);
                continue;
            }
            context.RetainedDefense.Add(result);
        }
        return context;
    }

    public ShootAction(Weapon _weapon, LayerMask _targetMask)
    {
        weapon = _weapon;
        layerMask = _targetMask;
    }

    public bool Declare(Vector3 point)
    {
        if (!CheckForTarget()) return false;
        return Perform(point);
    }

    public bool Perform(Vector3 point)
    {
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
        return true;
    }
}
