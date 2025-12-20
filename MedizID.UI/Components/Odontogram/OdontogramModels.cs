namespace MedizID.UI.Components.Odontogram;

public class ToothCondition
{
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
}

public class ToothState
{
    public int Number { get; set; }
    public string Surface { get; set; }
    public string ConditionCode { get; set; }
}
