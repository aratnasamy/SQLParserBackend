using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(ConditionItem), "ConditionItem")]
[JsonDerivedType(typeof(BetweenConditionItem), "BetweenConditionItem")]
[JsonDerivedType(typeof(CompoundConditionItem), "CompoundConditionItem")]
[JsonDerivedType(typeof(ExistsConditionItem), "ExistsConditionItem")]
[JsonDerivedType(typeof(FloatingPointConditionItem), "FloatingPointConditionItem")]
[JsonDerivedType(typeof(GroupComparisonConditionItem), "GroupComparisonConditionItem")]
[JsonDerivedType(typeof(InConditionItem), "InConditionItem")]
[JsonDerivedType(typeof(LikeConditionItem), "LikeConditionItem")]
[JsonDerivedType(typeof(LogicalConditionItem), "LogicalConditionItem")]
[JsonDerivedType(typeof(NullConditionItem), "NullConditionItem")]
[JsonDerivedType(typeof(RegexLikeConditionItem), "RegexLikeConditionItem")]
[JsonDerivedType(typeof(SimpleComparisonConditionItem), "SimpleComparisonConditionItem")]
public interface IConditionItem
{}