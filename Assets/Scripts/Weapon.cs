using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public int WeaponRange { get; private set; }
    public int WeaponAttack {  get; private set; }
    public int WeaponSkill {  get; private set; }
    public int WeaponDamage {  get; private set; }
    public List<ISpecialRule> SpecialRules {  get; private set; }

    private Weapon() { }

    public class Builder
    {
        readonly Weapon weapon;
        public Builder()
        {
            weapon = new Weapon();
        }
        public Builder WithRange(int range)
        {
            weapon.WeaponRange = range;
            return this;
        }
        public Builder WithAttack(int attack)
        {
            weapon.WeaponAttack = attack;
            return this;
        }
        public Builder WithSkill(int skill)
        {
            weapon.WeaponSkill = skill;
            return this;
        }
        public Builder WithDamage(int damage)
        {
            weapon.WeaponDamage = damage;
            return this;
        }
        public Builder WithSpecialRule(ISpecialRule specialRule)
        {
            weapon.SpecialRules.Add(specialRule);
            return this;
        }
        public Builder WithSpecialRules(List<ISpecialRule> specialRules)
        {
            weapon.SpecialRules = specialRules;
            return this;
        }
        public Weapon Build()
        {
            return weapon;
        }
    }
}
