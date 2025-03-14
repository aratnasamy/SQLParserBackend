using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(ExpressionItem), "ExpressionItem")]
[JsonDerivedType(typeof(SimpleExpressionItem), "SimpleExpressionItem")]
[JsonDerivedType(typeof(CaseExpressionItem), "CaseExpressionItem")]
[JsonDerivedType(typeof(CompoundExpressionItem), "CompoundExpressionItem")]
[JsonDerivedType(typeof(FunctionExpressionItem), "FunctionExpressionItem")]
[JsonDerivedType(typeof(OperatorExpressionItem), "OperatorExpressionItem")]
[JsonDerivedType(typeof(ScalarSubQueryExpressionItem), "ScalarSubQueryExpressionItem")]
[JsonDerivedType(typeof(SearchedCaseExpressionItem), "SearchedCaseExpressionItem")]
[JsonDerivedType(typeof(SimpleCaseExpressionItem), "SimpleCaseExpressionItem")]
public interface IExpressionItem
{
    string? ToString();
}