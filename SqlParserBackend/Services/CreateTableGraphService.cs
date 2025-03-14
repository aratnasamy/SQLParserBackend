public class CreateTableGraphService : ICreateTableGraphService
{
    public List<Dictionary<string,TableNode>> CreateTableGraph(List<Query> queryList)
    {
        List<Dictionary<string,TableNode>> res = [];
        foreach (Query query in queryList) {
            Dictionary<string,TableNode> queryGraph = [];
            
            res.Add(queryGraph);
        }
        return res;
    }
    public Dictionary<string,TableNode> QueryGraph(Query query)
    {
        Dictionary<string,TableNode> queryGraph = [];
        // with subqueries
        foreach (WithItem withItem in query.withItems) {
            queryGraph[withItem.alias!] = new TableNode(QueryGraph(withItem.subQuery!));
        }
        // select subqueries
        // TODO expressions and conditions need toString()s so in case there is no alias we can use toString for key
        foreach (SelectItem selectItem in query.selectItems) {
            if (selectItem.expression != null) {
                foreach (Query q in ExpressionQueries(selectItem.expression)) {
                    queryGraph[q.ToString()] = new TableNode(QueryGraph(q!));
                }
            }
        }
        // sources subqueries
        foreach (List<SourceItem> sourceList in query.sources) {
            foreach (SourceItem sourceItem in sourceList) {
                if (sourceItem.alias != null) { // aliased
                    if (queryGraph.ContainsKey(sourceItem.alias)) {
                        // alias collision
                        continue;
                    }
                    if (sourceItem.table?.Length > 0) { // table
                        queryGraph[sourceItem.alias] = new TableNode(sourceItem.table);
                    }
                    else { // subquery
                        queryGraph[sourceItem.alias!] = new TableNode(QueryGraph(sourceItem.query!));
                    }
                }
                else {
                    if (sourceItem.table?.Length > 0) { // table
                        queryGraph[sourceItem.table!] = new TableNode();
                    }
                    else { // subquery
                        queryGraph[sourceItem.query.ToString()] = new TableNode(QueryGraph(sourceItem.query!));
                    }
                }
            }
        }
        return queryGraph;
    }
    private List<Query> ExpressionQueries(IExpressionItem expressionItem)
    {
        List<Query> res = [];
        if (expressionItem == null) {
            return res;
        }
        if (expressionItem.GetType() == typeof(ExpressionItem)) {
            ExpressionItem castedExpression = (ExpressionItem)expressionItem;
            foreach (IExpressionItem expression in castedExpression.expressionItems) {
                res.AddRange(ExpressionQueries(expression));
            }
            return res;
        }
        if (expressionItem.GetType() == typeof(SimpleExpressionItem)) {
            return res;
        }
        if (expressionItem.GetType() == typeof(CaseExpressionItem)) {
            CaseExpressionItem castedExpression = (CaseExpressionItem)expressionItem;
            res.AddRange(ExpressionQueries(castedExpression.caseExpression));
            res.AddRange(ExpressionQueries(castedExpression.elseExpression));
            return res;
        }
        if (expressionItem.GetType() == typeof(SimpleCaseExpressionItem)) {
            SimpleCaseExpressionItem castedExpression = (SimpleCaseExpressionItem)expressionItem;
            res.AddRange(ExpressionQueries(castedExpression.expression));
            foreach ((IExpressionItem,IExpressionItem) pair in castedExpression.pairs) {
                res.AddRange(ExpressionQueries(pair.Item1));
                res.AddRange(ExpressionQueries(pair.Item2));
            }
            return res;
        }
        if (expressionItem.GetType() == typeof(SearchedCaseExpressionItem)) {
            SearchedCaseExpressionItem castedExpression = (SearchedCaseExpressionItem)expressionItem;
            foreach ((IConditionItem,IExpressionItem) pair in castedExpression.pairs) {
                res.AddRange(ConditionQueries(pair.Item1));
                res.AddRange(ExpressionQueries(pair.Item2));
            }
            return res;
        }
        if (expressionItem.GetType() == typeof(ScalarSubQueryExpressionItem)) {
            ScalarSubQueryExpressionItem castedExpression = (ScalarSubQueryExpressionItem)expressionItem;
            res.Add(castedExpression.subQuery!);
            return res;
        }
        if (expressionItem.GetType() == typeof(OperatorExpressionItem)) {
            return res;
        }
        if (expressionItem.GetType() == typeof(FunctionExpressionItem)) {
            FunctionExpressionItem castedExpression = (FunctionExpressionItem)expressionItem;
            foreach(IExpressionItem expression in castedExpression.parameters) {
                res.AddRange(ExpressionQueries(expression));
            }
            return res;
        }
        if (expressionItem.GetType() == typeof(CompoundExpressionItem)) {
            CompoundExpressionItem castedExpression = (CompoundExpressionItem)expressionItem;
            res.AddRange(ExpressionQueries(castedExpression.expression!));
            return res;
        }
        return res;
    }
    private List<Query> ConditionQueries(IConditionItem conditionItem)
    {
        List<Query> res = [];
        if (conditionItem == null) {
            return res;
        }
        if (conditionItem.GetType() == typeof(ConditionItem)) {
            ConditionItem castedCondition = (ConditionItem)conditionItem;
            foreach (IConditionItem condition in castedCondition.conditionItems) {
                res.AddRange(ConditionQueries(condition));
            }
            return res;
        }
        if (conditionItem.GetType() == typeof(BetweenConditionItem)) {
            BetweenConditionItem castedCondition = (BetweenConditionItem)conditionItem;
            res.AddRange(ExpressionQueries(castedCondition.expression1));
            res.AddRange(ExpressionQueries(castedCondition.expression2));
            res.AddRange(ExpressionQueries(castedCondition.expression3));
            return res;
        }
        if (conditionItem.GetType() == typeof(CompoundConditionItem)) {
            CompoundConditionItem castedCondition = (CompoundConditionItem)conditionItem;
            res.AddRange(ConditionQueries(castedCondition.condition));
            return res;
        }
        if (conditionItem.GetType() == typeof(ExistsConditionItem)) {
            ExistsConditionItem castedCondition = (ExistsConditionItem)conditionItem;
            res.Add(castedCondition.subQuery);
            return res;
        }
        if (conditionItem.GetType() == typeof(FloatingPointConditionItem)) {
            FloatingPointConditionItem castedCondition = (FloatingPointConditionItem)conditionItem;
            res.AddRange(ExpressionQueries(castedCondition.conditionExpression));
            return res;
        }
        if (conditionItem.GetType() == typeof(GroupComparisonConditionItem)) {
            GroupComparisonConditionItem castedCondition = (GroupComparisonConditionItem)conditionItem;
            foreach (IExpressionItem expressionItem in castedCondition.leftExpressionItems) {
                res.AddRange(ExpressionQueries(expressionItem));
            }
            if (castedCondition.subquery != null) {
                res.Add(castedCondition.subquery);
            }
            else {
                foreach (List<IExpressionItem> expressionList in castedCondition.rightExpressionItems) {
                    foreach (IExpressionItem expressionItem in expressionList) {
                        res.AddRange(ExpressionQueries(expressionItem));
                    }
                }
            }
            return res;
        }
        if (conditionItem.GetType() == typeof(InConditionItem)) {
            InConditionItem castedCondition = (InConditionItem)conditionItem;
            foreach (IExpressionItem expressionItem in castedCondition.leftExpressionItem) {
                res.AddRange(ExpressionQueries(expressionItem));
            }
            if (castedCondition.subQuery != null) {
                res.Add(castedCondition.subQuery);
            }
            else {
                foreach (List<IExpressionItem> expressionList in castedCondition.rightExpressionItems) {
                    foreach (IExpressionItem expressionItem in expressionList) {
                        res.AddRange(ExpressionQueries(expressionItem));
                    }
                }
            }
            return res;
        }
        if (conditionItem.GetType() == typeof(LikeConditionItem)) {
            return res;
        }
        if (conditionItem.GetType() == typeof(LogicalConditionItem)) {
            return res;
        }
        if (conditionItem.GetType() == typeof(NullConditionItem)) {
            NullConditionItem castedCondition = (NullConditionItem)conditionItem;
            res.AddRange(ExpressionQueries(castedCondition.conditionExpression));
            return res;
        }
        if (conditionItem.GetType() == typeof(RegexLikeConditionItem)) {
            return res;
        }
        if (conditionItem.GetType() == typeof(SimpleComparisonConditionItem)) {
            SimpleComparisonConditionItem castedCondition = (SimpleComparisonConditionItem)conditionItem;
            foreach (IExpressionItem expressionItem in castedCondition.leftExpressionItems) {
                res.AddRange(ExpressionQueries(expressionItem));
            }
            if (castedCondition.subquery != null) {
                res.Add(castedCondition.subquery);
            }
            else {
                foreach (IExpressionItem expressionItem in castedCondition.rightExpressionItems) {
                    res.AddRange(ExpressionQueries(expressionItem));
                }
            }
            return res;
        }
        return res;
    }
}