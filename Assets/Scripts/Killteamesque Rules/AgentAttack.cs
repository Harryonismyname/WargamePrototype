using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;
    Weapon weapon;
    public void Attack()
    {
        if (Physics.SphereCast(transform.position, 2, transform.forward, out RaycastHit hitInfo, weapon.WeaponRange, targetMask) && hitInfo.rigidbody.TryGetComponent(out AgentDefence target))
        {
            CombatContext context = new(weapon, target.Agent);
            RollForAttack(context);
            target.RollForDefence(context);
            foreach (var rule in weapon.SpecialRules)
            {
                context = rule.ApplyRule(context);
            }
        }
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
}