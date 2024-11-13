using System.Collections.Generic;

public class CombatContext
{
    public Weapon AttackingWeapon;
    public Agent Target;
    public List<int> RetainedHits;
    public List<int> RetainedCriticalHits;
    public List<int> RetainedDefence;
    public List<int> RetainedCriticalDefence;
    public List<int> SuccessfulHits;

    public CombatContext(Weapon _attackingWeapon, Agent _target)
    {
        AttackingWeapon= _attackingWeapon;
        Target= _target;
    }
}
