using UnityEngine;

public class Datasheet
{
    public int Wounds { get; set; }
    public int Defence { get; set; }
    public int Save { get; set; }
    public int AP { get; set; }
    public int APL { get; set; }
    public int Movement { get; set; }

    private Datasheet() { }

    public void ConsumeAP(int AP)
    {
        this.AP = Mathf.Clamp(this.AP - AP, 0, APL);
    }
    public void ResetAP()
    {
        AP = APL;
    }
    public class Builder
    {
        private readonly Datasheet sheet = new();
        public Builder SetDefense(int amount)
        {
            sheet.Defence = amount;
            return this;
        }
        public Builder SetSave(int amount)
        {
            sheet.Save = amount;
            return this;
        }
        public Builder SetAP(int amount)
        {
            sheet.AP = amount;
            return this;
        }
        public Builder SetAPL(int amount)
        {
            sheet.APL = amount;
            return this;
        }
        public Builder SetMovement(int amount)
        {
            sheet.Movement = amount;
            return this;
        }
        public Builder SetWounds(int amount)
        {
            sheet.Wounds = amount;
            return this;
        }
        public Datasheet Build()
        {
            return sheet;
        }
    }
}
