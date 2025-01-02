using UnityEngine;

public class AgentDefence : MonoBehaviour
{
    public Agent Agent;

    public CombatContext RollForDefence(CombatContext context)
    {
        int result;
        for (int i = 0; i < Agent.Defence; i++)
        {
            result = Random.Range(1, 6);
            if (result < Agent.Save) continue;
            if (result == 6)
            {
                context.RetainedCriticalDefense.Add(result);
                continue;
            }
            context.RetainedDefense.Add(result);
        }
        return context;
    }
}