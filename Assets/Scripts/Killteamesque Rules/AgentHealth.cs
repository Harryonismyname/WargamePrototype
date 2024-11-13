using System;
using UnityEngine;

public class AgentHealth : MonoBehaviour, IDamagable, IHealable
{
    [SerializeField] HealthData healthData;
    public Health Status { get; private set; }

    public event Action OnHealthChanged
    {
        add { Status.OnHealthChanged += value; }
        remove { Status.OnHealthChanged -= value; }
    }

    public event Action OnInjuredStatusChanged
    {
        add { Status.OnInjuredStatusChanged += value; }
        remove { Status.OnInjuredStatusChanged -= value; } 
    }

    public event Action OnDeath
    {
        add { Status.OnDeath += value; }
        remove { Status.OnDeath -= value;} 
    }

    private void Awake() => Status = healthData.GenerateData();

    public void DealDamage(int damage) => Status.DecreaseHealth(damage); 

    public void Heal(int amount) => Status.IncreaseHealth(amount);
}
