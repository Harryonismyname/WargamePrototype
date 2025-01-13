using UnityEngine;

[CreateAssetMenu(fileName = "NewDatasheet", menuName = "ScriptableObjects/Datasheet")]
public class DatasheetTemplate : ScriptableObject
{
    [SerializeField] int Wounds;
    [SerializeField] int Defense;
    [SerializeField] int Save;
    [SerializeField] int AP;
    [SerializeField] int Movement;
    public Datasheet GenerateData()
    {
        return new Datasheet.Builder()
            .SetWounds(Wounds)
            .SetDefense(Defense)
            .SetSave(Save)
            .SetAP(0)
            .SetAPL(AP)
            .SetMovement(Movement)
            .Build();
    }
}
