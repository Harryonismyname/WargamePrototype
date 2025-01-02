using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponTemplate : ScriptableObject
{
    [SerializeField] int Attack;
    [SerializeField] int Range;
    [SerializeField] int WeaponSkill;
    [SerializeField] int Damage;
    public Weapon GenerateData()
    {
        return new Weapon.Builder()
            .WithAttack(Attack)
            .WithRange(Range)
            .WithSkill(WeaponSkill)
            .WithDamage(Damage)
            .Build();
    }
}
