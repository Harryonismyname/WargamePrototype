using System;

public class Health
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public bool IsInjured => CurrentHealth <= MaxHealth * .5f;
    public bool IsDead => CurrentHealth <= 0;

    public event Action OnHealthChanged;
    public event Action OnInjuredStatusChanged;
    public event Action OnDeath;

    public Health(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }

    public void DecreaseHealth(int amount)
    {
        if(IsDead) return;
        CurrentHealth-=amount;
        OnHealthChanged?.Invoke();
        if (IsInjured) OnInjuredStatusChanged?.Invoke();
        if (IsDead) OnDeath?.Invoke();
    }

    public void IncreaseHealth(int amount)
    {
        if (IsInjured && CurrentHealth + amount > MaxHealth * .5f) OnInjuredStatusChanged?.Invoke();
        CurrentHealth+=amount;
        OnHealthChanged?.Invoke();
    }

}
