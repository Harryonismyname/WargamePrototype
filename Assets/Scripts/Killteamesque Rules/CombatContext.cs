using PolearmStudios.Utils;
using System.Collections.Generic;
using System.Linq;

public class CombatContext
{
    public Weapon AttackingWeapon;
    public Agent Target;
    public List<int> RetainedHits = new();
    public List<int> RetainedCriticalHits = new();
    public List<int> RetainedDefense = new();
    public List<int> RetainedCriticalDefense = new();
    public List<int> SuccessfulHits = new();
    public List<int> SuccessfulCrits = new();

    public CombatContext(Weapon _attackingWeapon, Agent _target)
    {
        AttackingWeapon= _attackingWeapon;
        Target= _target;
    }

    public void DetermineSuccessfulHits()
    {
        RetainedHits = Utilities.QuickSort(RetainedHits);
        RetainedDefense = Utilities.QuickSort(RetainedDefense);
        RetainedCriticalHits = Utilities.QuickSort(RetainedCriticalHits);
        RetainedCriticalDefense = Utilities.QuickSort(RetainedCriticalDefense);
        if (RetainedHits.Count < 1 && RetainedCriticalHits.Count < 1) return;
        if (RetainedDefense.Count < 1 && RetainedCriticalDefense.Count < 1)
        {
            SuccessfulHits = RetainedHits;
            SuccessfulCrits= RetainedCriticalHits;
            return;
        }
        // Copy block lists to track remaining unused blocks
        var remainingCriticalBlocks = RetainedCriticalDefense.ToList();
        var remainingNormalBlocks = RetainedDefense.ToList();

        // Unblocked critical hits
        var unblockedCriticalHits = RetainedCriticalHits
            .Where(critHit =>
            {
                var block = remainingCriticalBlocks.FirstOrDefault(critBlock => critBlock >= critHit);
                if (block != default)
                {
                    remainingCriticalBlocks.Remove(block); // Consume the block
                    return false; // Hit is blocked
                }
                return true; // Hit is unblocked
            })
            .ToList();

        // Unblocked normal hits
        var unblockedNormalHits = RetainedHits
            .Where(hit =>
            {
                var block = remainingCriticalBlocks.FirstOrDefault(critBlock => critBlock >= hit);
                var criticalBlock = remainingNormalBlocks.FirstOrDefault(block => block >= hit);
                // Check if a valid critical block is found
                if (criticalBlock > 0)
                {
                    remainingCriticalBlocks.Remove(criticalBlock); // Consume critical block
                    return false; // Hit is blocked by a critical block
                }

                // Check if a valid normal block is found
                if (block > 0)
                {
                    remainingNormalBlocks.Remove(block); // Consume normal block
                    return false; // Hit is blocked by a normal block
                }

                return true; // Hit is unblocked
            })
            .ToList();
        SuccessfulCrits = unblockedCriticalHits == null ? SuccessfulCrits : SuccessfulCrits.Concat(unblockedCriticalHits).ToList();
        SuccessfulHits  = unblockedNormalHits   == null ? SuccessfulHits  : SuccessfulHits .Concat(unblockedNormalHits)  .ToList();
    }
}
