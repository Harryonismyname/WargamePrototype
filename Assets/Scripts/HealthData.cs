using UnityEngine;

[CreateAssetMenu(fileName ="NewHealthData", menuName = "ScriptableObjects/HealthData")]
public class HealthData: ScriptableObject
{
    [SerializeField] int MaxHealth = 5;
    public Health GenerateData()
    {
        return new Health(MaxHealth);
    }
}
