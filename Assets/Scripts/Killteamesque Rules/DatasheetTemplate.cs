using UnityEngine;

[CreateAssetMenu(fileName = "NewDatasheet", menuName = "ScriptableObjects/Datasheet")]
public class DatasheetTemplate : ScriptableObject
{
    [SerializeField] int Wounds;
    [SerializeField] int Defence;
    [SerializeField] int Save;
    [SerializeField] int AP;
    [SerializeField] int Movement;
    public Datasheet GenerateData()
    {
        return new Datasheet.Builder()
            .SetWounds(Wounds)
            .SetDefence(Defence)
            .SetSave(Save)
            .SetAP(0)
            .SetAPL(AP)
            .SetMovement(Movement)
            .Build();
    }
}
