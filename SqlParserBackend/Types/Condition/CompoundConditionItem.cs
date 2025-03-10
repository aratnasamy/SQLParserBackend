public class CompoundConditionItem : IConditionItem
{
    public string? logic { get; set; }
    public IConditionItem? condition { get; set; }
}