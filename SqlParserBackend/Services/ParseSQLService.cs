
public class ParseSQLService : IParseSQLService
{
    public List<Query> Parse(string sql)
    {
        return Query(Tokenize(sql));
    }
    private List<string> Tokenize(string input)
    {
        List<string> tokens = new List<string>{""};
        bool doubleQuoteFlag = false;
        bool singleQuoteFlag = false;
        foreach (char c in input)
        {
            if(c == ' ') {
                if(doubleQuoteFlag || singleQuoteFlag) {
                    tokens[^1] += c;
                }
                else {
                    tokens.Add("");
                }
            }
            else if(c == '"' && !singleQuoteFlag) {
                if(doubleQuoteFlag) {
                    tokens[^1] += c;
                    tokens.Add("");
                    doubleQuoteFlag = false;
                }
                else {
                    tokens.Add(c.ToString());
                    doubleQuoteFlag = true;
                }
            }
            else if(c == '\'' && ! doubleQuoteFlag) {
                if(singleQuoteFlag) {
                    tokens[^1] += c;
                    tokens.Add("");
                    singleQuoteFlag = false;
                }
                else {
                    tokens.Add(c.ToString());
                    singleQuoteFlag = true;
                }
            }
            else if("*,.();+-<!^".Contains(c)) {
                if(doubleQuoteFlag || singleQuoteFlag) {
                    tokens[^1] += c;
                }
                else {
                    tokens.Add(c.ToString());
                    tokens.Add("");
                }
            }
            else if(c == '=') {
                if(tokens[^1] == "!" || tokens[^1] == "^" || tokens[^1] == "<" || tokens[^1] == ">" || doubleQuoteFlag || singleQuoteFlag) {
                    tokens[^1] += c;
                }
                else {
                    tokens.Add("=");
                }
            }
            else if(c == '>') {
                if(tokens[^1] == "<" || doubleQuoteFlag || singleQuoteFlag) {
                    tokens[^1] += c;
                }
                else {
                    tokens.Add(">");
                }
            }
            else if(c == '|') {
                if(tokens[^1] == "|" || doubleQuoteFlag || singleQuoteFlag) {
                    tokens[^1] += c;
                }
                else {
                    tokens.Add("|");
                }
            }
            else if(c == '.') {
                if(IsInteger(tokens[^1]) || doubleQuoteFlag || singleQuoteFlag) {
                    tokens[^1] += c;
                }
                else {
                    tokens.Add(c.ToString());
                }
            }
            else if(IsNumber(tokens[^1]) && !"0123456789".Contains(c)) {
                tokens.Add(c.ToString());
            }
            else {
                tokens[^1] += c;
            }
        }
        tokens.Add("#EOF");
        return tokens.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
    }
    private bool IsInteger(string i)
    {
        foreach (char c in i) {
            if(!"0123456789".Contains(c)) {
                return false;
            }
        }
        return true;
    }
    private bool IsNumber(string i)
    {
        int dCount = 0;
        foreach (char c in i) {
            if(c == '.') {
                dCount++;
                if(dCount > 1) {
                    return false;
                }
            }
            else if(!"0123456789".Contains(c)) {
                return false;
            }
        }
        return true;
    }
    readonly Dictionary<string,int> ReservedWords = new Dictionary<string, int>
    {
        {"ACCESS",0},
        {"ADD",0},
        {"ALL",0},
        {"ALTER",0},
        {"AND",0},
        {"ANY",0},
        {"ARRAYLEN",0},
        {"AS",0},
        {"ASC",0},
        {"AUDIT",0},
        {"BETWEEN",0},
        {"BY",0},
        {"CHAR",0},
        {"CHECK",0},
        {"CLUSTER",0},
        {"COLUMN",0},
        {"COMMENT",0},
        {"COMPRESS",0},
        {"CONNECT",0},
        {"CREATE",0},
        {"CURRENT",0},
        {"DATE",0},
        {"DECIMAL",0},
        {"DEFAULT",0},
        {"DELETE",0},
        {"DESC",0},
        {"DISTINCT",0},
        {"DROP",0},
        {"ELSE",0},
        {"EXCLUSIVE",0},
        {"EXISTS",0},
        {"FILE",0},
        {"FLOAT",0},
        {"FOR",0},
        {"FROM",0},
        {"GRANT",0},
        {"GROUP",0},
        {"HAVING",0},
        {"IDENTIFIED",0},
        {"IMMEDIATE",0},
        {"IN",0},
        {"INCREMENT",0},
        {"INDEX",0},
        {"INITIAL",0},
        {"INSERT",0},
        {"INTEGER",0},
        {"INTERSECT",0},
        {"INTO",0},
        {"IS",0},
        {"LEVEL",0},
        {"LIKE",0},
        {"LOCK",0},
        {"LONG",0},
        {"MAXEXTENTS",0},
        {"MINUS",0},
        {"MODE",0},
        {"MODIFY",0},
        {"NOAUDIT",0},
        {"NOCOMPRESS",0},
        {"NOT",0},
        {"NOTFOUND",0},
        {"NOWAIT",0},
        {"NULL",0},
        {"NUMBER",0},
        {"OF",0},
        {"OFFLINE",0},
        {"ON",0},
        {"ONLINE",0},
        {"OPTION",0},
        {"OR",0},
        {"ORDER",0},
        {"PCTFREE",0},
        {"PRIOR",0},
        {"PRIVILEGES",0},
        {"PUBLIC",0},
        {"RAW",0},
        {"RENAME",0},
        {"RESOURCE",0},
        {"REVOKE",0},
        {"ROW",0},
        {"ROWID",0},
        {"ROWLABEL",0},
        {"ROWNUM",0},
        {"ROWS",0},
        {"START",0},
        {"SELECT",0},
        {"SESSION",0},
        {"SET",0},
        {"SHARE",0},
        {"SIZE",0},
        {"SMALLINT",0},
        {"SQLBUF",0},
        {"SUCCESSFUL",0},
        {"SYNONYM",0},
        {"SYSDATE",0},
        {"TABLE",0},
        {"THEN",0},
        {"TO",0},
        {"TRIGGER",0},
        {"UID",0},
        {"UNION",0},
        {"UNIQUE",0},
        {"UPDATE",0},
        {"USER",0},
        {"VALIDATE",0},
        {"VALUES",0},
        {"VARCHAR",0},
        {"VARCHAR2",0},
        {"VIEW",0},
        {"WHENEVER",0},
        {"WHERE",0},
        {"WITH",0}
    };
    readonly Dictionary<string,int> Functions = new Dictionary<string, int>
    {
        {"ABS",0},
        {"ACOS",0},
        {"ADD_MONTHS",0},
        {"APPROX_COUNT_DISTINCT_AGG",0},
        {"APPROX_COUNT_DISTINCT_DETAIL",0},
        {"APPROX_COUNT_DISTINCT",0},
        {"APPROX_COUNT",0},
        {"APPROX_MEDIAN",0},
        {"APPROX_PERCENTILE_AGG",0},
        {"APPROX_PERCENTILE_DETAIL",0},
        {"APPROX_PERCENTILE",0},
        {"APPROX_RANK",0},
        {"APPROX_SUM",0},
        {"ASCII",0},
        {"ASCIISTR",0},
        {"ASIN",0},
        {"ATAN",0},
        {"ATAN2",0},
        {"AVG",0},
        {"BFILENAME",0},
        {"BIN_TO_NUM",0},
        {"BITAND",0},
        {"CARDINALITY",0},
        {"CAST",0},
        {"CEIL",0},
        {"CHARTOROWID",0},
        {"CHR",0},
        {"CLUSTER_DETAILS",0},
        {"CLUSTER_DISTANCE",0},
        {"CLUSTER_ID",0},
        {"CLUSTER_PROBABILITY",0},
        {"CLUSTER_SET",0},
        {"COALESCE",0},
        {"COLLATION",0},
        {"COLLECT",0},
        {"COMPOSE",0},
        {"CON_DBID_TO_ID",0},
        {"CON_GUID_TO_ID",0},
        {"CON_NAME_TO_ID",0},
        {"CON_UID_TO_ID",0},
        {"CONCAT",0},
        {"CONVERT",0},
        {"CORR_K",0},
        {"CORR_S",0},
        {"CORR",0},
        {"COS",0},
        {"COSH",0},
        {"COUNT",0},
        {"COVAR_POP",0},
        {"COVAR_SAMP",0},
        {"CUME_DIST",0},
        {"CURRENT_DATE",0},
        {"CURRENT_TIMESTAMP",0},
        {"DBTIMEZONE",0},
        {"DECODE",0},
        {"DECOMPOSE",0},
        {"DENSE_RANK",0},
        {"DEPTH",0},
        {"DUMP",0},
        {"EMPTY_BLOB",0},
        {"EMPTY_CLOB",0},
        {"EXISTSNODE",0},
        {"EXP",0},
        {"EXTRACT",0},
        {"EXTRACTVALUE",0},
        {"FEATURE_COMPARE",0},
        {"FEATURE_DETAILS",0},
        {"FEATURE_ID",0},
        {"FEATURE_SET",0},
        {"FEATURE_VALUE",0},
        {"FIRST",0},
        {"FLOOR",0},
        {"FROM_TZ",0},
        {"GREATEST",0},
        {"GROUP_ID",0},
        {"GROUPING_ID",0},
        {"GROUPING",0},
        {"HEXTORAW",0},
        {"INITCAP",0},
        {"INSTR",0},
        {"JSON_ARRAY",0},
        {"JSON_ARRAYAGG",0},
        {"JSON_DATAGUIDE",0},
        {"JSON_OBJECT",0},
        {"JSON_OBJECTAGG",0},
        {"JSON_QUERY",0},
        {"JSON_TABLE",0},
        {"JSON_VALUE",0},
        {"LAST_DAY",0},
        {"LAST",0},
        {"LEAST",0},
        {"LENGTH",0},
        {"LISTAGG",0},
        {"LN",0},
        {"LNNVL",0},
        {"LOCALTIMESTAMP",0},
        {"LOG",0},
        {"LOWER",0},
        {"LPAD",0},
        {"LTRIM",0},
        {"MAX",0},
        {"MEDIAN",0},
        {"MIN",0},
        {"MOD",0},
        {"MONTHS_BETWEEN",0},
        {"NANVL",0},
        {"NCHR",0},
        {"NEW_TIME",0},
        {"NEXT_DAY",0},
        {"NLS_CHARSET_DECL_LEN",0},
        {"NLS_CHARSET_ID",0},
        {"NLS_CHARSET_NAME",0},
        {"NLS_COLLATION_ID",0},
        {"NLS_COLLATION_NAME",0},
        {"NLS_INITCAP",0},
        {"NLS_LOWER",0},
        {"NLS_UPPER",0},
        {"NLSSORT",0},
        {"NULLIF",0},
        {"NUMTODSINTERVAL",0},
        {"NUMTOYMINTERVAL",0},
        {"NVL",0},
        {"NVL2",0},
        {"ORA_DM_PARTITION_NAME",0},
        {"ORA_DST_AFFECTED",0},
        {"ORA_DST_CONVERT",0},
        {"ORA_DST_ERROR",0},
        {"ORA_HASH",0},
        {"ORA_INVOKING_USER",0},
        {"ORA_INVOKING_USERID",0},
        {"PATH",0},
        {"PERCENT_RANK",0},
        {"PERCENTILE_CONT",0},
        {"PERCENTILE_DISC",0},
        {"POWER",0},
        {"POWERMULTISET_BY_CARDINALITY",0},
        {"POWERMULTISET",0},
        {"PREDICTION_BOUNDS",0},
        {"PREDICTION_COST",0},
        {"PREDICTION_DETAILS",0},
        {"PREDICTION_PROBABILITY",0},
        {"PREDICTION_SET",0},
        {"PREDICTION",0},
        {"RANK",0},
        {"RAWTOHEX",0},
        {"RAWTONHEX",0},
        {"REGEXP_COUNT",0},
        {"REGEXP_INSTR",0},
        {"REGEXP_REPLACE",0},
        {"REGEXP_SUBSTR",0},
        {"REGR_",0},
        {"REMAINDER",0},
        {"REPLACE",0},
        {"ROUND",0},
        {"ROWIDTOCHAR",0},
        {"ROWIDTONCHAR",0},
        {"RPAD",0},
        {"RTRIM",0},
        {"SCN_TO_TIMESTAMP",0},
        {"SESSIONTIMEZONE",0},
        {"SET",0},
        {"SIGN",0},
        {"SIN",0},
        {"SINH",0},
        {"SOUNDEX",0},
        {"SQRT",0},
        {"STANDARD_HASH",0},
        {"STATS_BINOMIAL_TEST",0},
        {"STATS_CROSSTAB",0},
        {"STATS_F_TEST",0},
        {"STATS_KS_TEST",0},
        {"STATS_MODE",0},
        {"STATS_MW_TEST",0},
        {"STATS_ONE_WAY_ANOVA",0},
        {"STATS_T_TEST_INDEP",0},
        {"STATS_T_TEST_INDEPU",0},
        {"STATS_T_TEST_ONE",0},
        {"STATS_T_TEST_PAIRED",0},
        {"STATS_WSR_TEST",0},
        {"STDDEV_POP",0},
        {"STDDEV_SAMP",0},
        {"STDDEV",0},
        {"SUBSTR",0},
        {"SUM",0},
        {"SYS_CONNECT_BY_PATH",0},
        {"SYS_CONTEXT",0},
        {"SYS_DBURIGEN",0},
        {"SYS_EXTRACT_UTC",0},
        {"SYS_GUID",0},
        {"SYS_OP_ZONE_ID",0},
        {"SYS_TYPEID",0},
        {"SYS_XMLAGG",0},
        {"SYS_XMLGEN",0},
        {"SYSDATE",0},
        {"SYSTIMESTAMP",0},
        {"TAN",0},
        {"TANH",0},
        {"TIMESTAMP_TO_SCN",0},
        {"TO_APPROX_COUNT_DISTINCT",0},
        {"TO_APPROX_PERCENTILE",0},
        {"TO_BINARY_DOUBLE",0},
        {"TO_BINARY_FLOAT",0},
        {"TO_BLOB",0},
        {"TO_CHAR",0},
        {"TO_CLOB",0},
        {"TO_DATE",0},
        {"TO_DSINTERVAL",0},
        {"TO_LOB",0},
        {"TO_MULTI_BYTE",0},
        {"TO_NCHAR",0},
        {"TO_NCLOB",0},
        {"TO_NUMBER",0},
        {"TO_SINGLE_BYTE",0},
        {"TO_TIMESTAMP_TZ",0},
        {"TO_TIMESTAMP",0},
        {"TO_YMINTERVAL",0},
        {"TRANSLATE",0},
        {"TREAT",0},
        {"TRIM",0},
        {"TRUNC",0},
        {"TZ_OFFSET",0},
        {"UID",0},
        {"UNISTR",0},
        {"UPPER",0},
        {"USER",0},
        {"USERENV",0},
        {"VALIDATE_CONVERSION",0},
        {"VAR_POP",0},
        {"VAR_SAMP",0},
        {"VARIANCE",0},
        {"VSIZE",0},
        {"WIDTH_BUCKET",0},
        {"XMLAGG",0},
        {"XMLCAST",0},
        {"XMLCDATA",0},
        {"XMLCOLATTVAL",0},
        {"XMLCOMMENT",0},
        {"XMLCONCAT",0},
        {"XMLDIFF",0},
        {"XMLELEMENT",0},
        {"XMLEXISTS",0},
        {"XMLFOREST",0},
        {"XMLISVALID",0},
        {"XMLPARSE",0},
        {"XMLPATCH",0},
        {"XMLPI",0},
        {"XMLQUERY",0},
        {"XMLROOT",0},
        {"XMLSEQUENCE",0},
        {"XMLSERIALIZE",0},
        {"XMLTABLE",0},
        {"XMLTRANSFORM",0}
    };
    private bool IsReserved(string value)
    {
        return ReservedWords.ContainsKey(value.ToUpper());
    }
    private bool IsFunction(string value)
    {
        return Functions.ContainsKey(value.ToUpper());
    }
    private bool IsValidIdentifier(string value)
    {
        if(value == "*") {
            return true;
        }
        if(value.Length > 128) {
            return false;
        }
        if(value[0] == '"' && value[^1] == '"' && value.Length > 2) {
            return true;
        }
        if(!char.IsLetter(value[0]) || !value.All(c => char.IsLetterOrDigit(c) || c == '$' || c == '_' || c == '#')) {
            return false;
        }
        if(IsReserved(value)) {
            return false;
        }
        return true;
    }
    private bool IsValidTable(string value)
    {
        if(value.Length > 30) {
            return false;
        }
        if(value[0] == '"' && value[^1] == '"' && value.Length > 2) {
            return true;
        }
        if(!char.IsLetter(value[0]) || !value.All(c => char.IsLetterOrDigit(c) || c == '$' || c == '_' || c == '#')) {
            return false;
        }
        if(IsReserved(value)) {
            return false;
        }
        return true;
    }
    private List<Query> Query(List<string> sql)
    {
        int ptr = 0;
        List<Query> ql = [];
        try
        {
            SubQuery(sql,ptr,out int x,out Query y);
            ptr = x;
            ql.Add(y);
            while(true) {
                string QueryOperator;
                if(sql[ptr].ToUpper() == "UNION") {
                    QueryOperator = "UNION";
                    ptr++;
                    if(sql[ptr].ToUpper() == "ALL") {
                        QueryOperator += " ALL";
                        ptr++;
                    }
                }
                else if(sql[ptr].ToUpper() == "INTERSECT" || sql[ptr].ToUpper() == "MINUS") {
                    QueryOperator = sql[ptr].ToUpper();
                    ptr++;
                }
                else {
                    return ql;
                }
                SubQuery(sql,ptr,out int x2,out Query y2);
                ptr = x2;
                y2.queryJoin = QueryOperator;
                ql.Add(y2);
            }
        }
        catch
        {
            return ql;
        }
    }
    private bool QueryBlock(List<string> sql, int inptr, out int outptr, out Query q)
    {
        outptr = inptr;
        q = new Query();
        try
        {
            if (WithClause(sql,outptr,out int x,out List<WithItem> y)) {
                outptr = x;
                q.withItems = y;
            }
            if(sql[outptr].ToUpper() != "SELECT") {
                q.error = "Missing SELECT";
                return false;
            }
            outptr++;
            if(sql[outptr].ToUpper() == "ALL" || sql[outptr].ToUpper() == "DISTINCT" || sql[outptr].ToUpper() == "UNIQUE") {
                q.selectModifier = sql[outptr].ToUpper();
                outptr++;
            }
            if (SelectList(sql,outptr,out int x2,out List<SelectItem> y2)) {
                outptr = x2;
                q.selectItems = y2;
            }
            if(sql[outptr].ToUpper() != "FROM") {
                q.error = "Missing FROM";
                return false;
            }
            outptr++;
            while(true) {
                bool objectFound = false;
                if(sql[outptr] == "(") {
                    outptr++;
                    if (JoinClause(sql,outptr,out int x3,out List<SourceItem> y3)) {
                        outptr = x3;
                        q.sources.Add(y3);
                        if(sql[outptr] == ")") {
                            outptr++;
                            objectFound = true;
                        }
                        else {
                            q.error = "Invalid Table Reference or Join clause";
                            return false;
                        }
                    }
                    outptr--;
                }
                if(!objectFound) {
                    if (JoinClause(sql,outptr,out int x4,out List<SourceItem> y4)) {
                        outptr = x4;
                        q.sources.Add(y4);
                        objectFound = true;
                    }
                }
                if(!objectFound) {
                    if (TableReference(sql,outptr,out int x5,out SourceItem y5)) {
                        outptr = x5;
                        q.sources.Add([y5]);
                        objectFound = true;
                    }
                }
                if(!objectFound) {
                    q.error = "Invalid Table Reference or Join clause";
                    return false;
                    }
                if(sql[outptr].ToUpper() == "WHERE" || sql[outptr] == "#EOF" || sql[outptr] == ";" || sql[outptr].ToUpper() == "UNION" || sql[outptr].ToUpper() == "INTERSECT" || sql[outptr].ToUpper() == "MINUS" || sql[outptr].ToUpper() == "GROUP") {
                    break;
                }
                else if(sql[outptr] == ",") {
                    outptr++;
                }
                else {
                    q.error = "Invalid Table Reference or Join clause";
                    return false;
                }
            }
            if (WhereClause(sql,outptr,out int x6,out IConditionItem y6)) {
                outptr = x6;
                q.whereCondition = y6;
            }
            if (GroupByClause(sql,outptr,out int x7,out List<IExpressionItem> y7,out IConditionItem z7)) {
                outptr = x7;
                q.groupByExpressions = y7;
                q.groupByCondition = z7;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool WithClause(List<string> sql, int inptr, out int outptr, out List<WithItem> wl)
    {
        outptr = inptr;
        wl = [];
        try
        {
            if(sql[outptr].ToUpper() == "WITH") {
                outptr++;
                while(true) {
                    WithItem w = new WithItem();
                    if(IsValidTable(sql[outptr])) {
                        w.alias = sql[outptr];
                        outptr++;
                    }
                    else {
                        return false;
                    }
                    if(sql[outptr] == "(") {
                        outptr++;
                        while(true) {
                            if(IsValidIdentifier(sql[outptr])) {
                                w.columnAliases.Add(sql[outptr]);
                                outptr++;
                            }
                            else {
                                return false;
                            }
                            if(sql[outptr] == ")") {
                                outptr++;
                                break;
                            }
                            else if(sql[outptr] == ",") {
                                outptr++;
                            }
                            else {
                                return false;
                            }
                        }
                    }
                    if(sql[outptr].ToUpper() == "AS") {
                        outptr++;
                    }
                    else {
                        return false;
                    }
                    if(sql[outptr] == "(") {
                        List<string> temp = [];
                        outptr++;
                        int parenthesesDepth = 1;
                        while(true) {
                            if(sql[outptr] == "(") {
                                parenthesesDepth++;
                            }
                            else if(sql[outptr] == ")") {
                                parenthesesDepth--;
                                if(parenthesesDepth == 0) {
                                    temp.Add("#EOF");
                                    outptr++;
                                    if (SubQuery(temp,0,out _,out Query y)) {
                                        w.subQuery = y;
                                        wl.Add(w);
                                        break;
                                    }
                                    else {
                                        return false;
                                    }
                                }
                            }
                            temp.Add(sql[outptr]);
                            outptr++;
                        }
                    }
                    else {
                        return false;
                    }
                    if(sql[outptr] == ",") {
                        outptr++;
                    }
                    else {
                        return true;
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool SelectList(List<string> sql, int inptr, out int outptr, out List<SelectItem> sl)
    {
        outptr = inptr;
        sl = [];
        try
        {
            SelectItem s = new SelectItem();
            if(sql[outptr] == "*") {
                s.field = sql[outptr];
                sl.Add(s);
                outptr++;
                return true;
            }
            while(true) {
                if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                    outptr = x;
                    s.expression = y;
                    if(sql[outptr].ToUpper() == "AS") {
                        outptr++;
                        if(IsValidIdentifier(sql[outptr])) {
                            s.alias = sql[outptr];
                            outptr++;
                        }
                        else {
                            return false;
                        }
                    }
                    else if(IsValidIdentifier(sql[outptr])) {
                        s.alias = sql[outptr];
                        outptr++;
                    }
                }
                else if(IsValidIdentifier(sql[outptr])) {
                    s.field = sql[outptr];
                    outptr++;
                    if(sql[outptr] == ".") {
                        outptr++;
                        if(IsValidTable(s.field) && IsValidIdentifier(sql[outptr])) {
                            s.table = s.field;
                            s.field = sql[outptr];
                            outptr++;
                            if(sql[outptr] == ".") {
                                outptr++;
                                if(IsValidTable(s.field) && IsValidIdentifier(sql[outptr])) {
                                    s.schema = s.table;
                                    s.table = s.field;
                                    s.field = sql[outptr];
                                    outptr++;
                                }
                                else {
                                    return false;
                                }
                            }
                        }
                        else {
                            return false;
                        }
                    }
                }
                else {
                    return false;
                }
                if(sql[outptr].ToUpper() == "FROM") {
                    sl.Add(s);
                    return true;
                }
                else if(sql[outptr] == ",") {
                    sl.Add(s);
                    s = new SelectItem();
                    outptr++;
                }
            }
        }
        catch
        {
            return false;
        }
    }
    private bool TableReference(List<string> sql, int inptr, out int outptr, out SourceItem s)
    {
        outptr = inptr;
        s = new SourceItem();
        try
        {
            if (QueryTableExpression(sql,outptr,out int x,out SourceItem y)) {
                outptr = x;
                s = y;
            }
            else if(sql[outptr] == "(") {
                outptr++;
                int parenthesesDepth = 1;
                List<string> temp = new List<string>();
                while(true) {
                    if(sql[outptr] == "(") {
                        parenthesesDepth++;
                    }
                    else if(sql[outptr] == ")") {
                        parenthesesDepth--;
                        if(parenthesesDepth == 0) {
                            temp.Add("#EOF");
                            outptr++;
                            break;
                        }
                    }
                    temp.Add(sql[outptr]);
                    outptr++;
                }
                if (SubQuery(temp,0,out _,out Query y2)) {
                    s.query = y2;
                    return true;
                }
                else if (TableReference(temp,0,out _,out SourceItem y3)) {
                    s = y3;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
            if (IsValidIdentifier(sql[outptr])) {
                s.alias = sql[outptr];
                outptr++;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool QueryTableExpression(List<string> sql, int inptr, out int outptr, out SourceItem s)
    {
        outptr = inptr;
        s = new SourceItem();
        try
        {
            if(IsValidTable(sql[outptr])) {
                s.table = sql[outptr];
                outptr++;
                if(sql[outptr] == ".") {
                    outptr++;
                    if(IsValidTable(sql[outptr])) {
                        s.schema = s.table;
                        s.table = sql[outptr];
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
            else if(sql[outptr] == "(") {
                outptr++;
                int parenthesesDepth = 1;
                List<string> temp = new List<string>();
                while(true) {
                    if(sql[outptr] == "(") {
                        parenthesesDepth++;
                    }
                    else if(sql[outptr] == ")") {
                        parenthesesDepth--;
                        if(parenthesesDepth == 0) {
                            temp.Add("#EOF");
                            outptr++;
                            break;
                        }
                    }
                    temp.Add(sql[outptr]);
                    outptr++;
                }
                if (SubQuery(temp,0,out _,out Query y)) {
                    s.query = y;
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    private bool SubQuery(List<string> sql, int inptr, out int outptr, out Query q)
    {
        outptr = inptr;
        q = new Query();
        try
        {
            if (QueryBlock(sql,outptr,out int x,out Query y)) {
                outptr = x;
                q = y;
                if (OrderByClause(sql,outptr,out int x2,out string y2,out List<OrderByItem> z2)) {
                    outptr = x2;
                    q.orderByModifier = y2;
                    q.orderByItems = z2;
                }
                if (RowLimitingClause(sql,outptr,out int x3,out string y3)) {
                    outptr = x3;
                    q.rowLimitingClause = y3;
                }
                return true;
            }
            if(sql[outptr] == "(") {
                outptr++;
                int parenthesesDepth = 1;
                List<string> temp = new List<string>();
                while(true) {
                    if(sql[outptr] == "(") {
                        parenthesesDepth++;
                    }
                    else if(sql[outptr] == ")") {
                        parenthesesDepth--;
                        if(parenthesesDepth == 0) {
                            temp.Add("#EOF");
                            outptr++;
                            break;
                        }
                    }
                    temp.Add(sql[outptr]);
                    outptr++;
                }
                if (SubQuery(temp,0,out _,out Query y4)) {
                    q = y4;
                    if (OrderByClause(sql,outptr,out int x2,out string y2,out List<OrderByItem> z2)) {
                        outptr = x2;
                        q.orderByModifier = y2;
                        q.orderByItems = z2;
                    }
                    if (RowLimitingClause(sql,outptr,out int x3,out string y3)) {
                        outptr = x3;
                        q.rowLimitingClause = y3;
                    }
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool JoinClause(List<string> sql, int inptr, out int outptr, out List<SourceItem> sl)
    {
        outptr = inptr;
        sl = [];
        try
        {
            if (TableReference(sql,outptr,out int x,out SourceItem y)) {
                outptr = x;
                sl.Add(y);
                while(true) {
                    if (InnerJoinClause(sql,outptr,out int x2,out SourceItem y2)) {
                        outptr = x2;
                        sl.Add(y2);
                    }
                    else if (OuterJoinClause(sql,outptr,out int x3,out SourceItem y3)) {
                        outptr = x3;
                        sl.Add(y3);
                    }
                    else {
                        return false;
                    }
                    if(sql[outptr] == "," || sql[outptr] == "#EOF" || sql[outptr].ToUpper() == "WHERE" || sql[outptr] == ";" || sql[outptr].ToUpper() == "UNION" || sql[outptr].ToUpper() == "INTERSECT" || sql[outptr].ToUpper() == "MINUS") {
                        return true;
                    }
                }
            }
            else {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    private bool InnerJoinClause(List<string> sql, int inptr, out int outptr, out SourceItem s)
    {
        outptr = inptr;
        s = new SourceItem();
        try
        {
            if(sql[outptr].ToUpper() == "INNER") {
                s.joinType += sql[outptr].ToUpper();
                outptr++;
                if(sql[outptr].ToUpper() == "JOIN") {
                    s.joinType += sql[outptr].ToUpper();
                    outptr++;
                    if (TableReference(sql,outptr,out int x,out SourceItem y)) {
                        outptr = x;
                        s.schema = y.schema;
                        s.table = y.table;
                        s.query = y.query;
                        s.alias = y.alias;
                        if(sql[outptr].ToUpper() == "ON") {
                            outptr++;
                            if (Condition(sql,outptr,out int x2,out IConditionItem y2)) {
                                outptr = x2;
                                s.joinCondition = y2;
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        else if(sql[outptr].ToUpper() == "USING") {
                            outptr++;
                            if(sql[outptr] == "(") {
                                outptr++;
                                while(true) {
                                    if (IsValidIdentifier(sql[outptr])) {
                                        s.joinOnColumns.Add(sql[outptr]);
                                        outptr++;
                                    }
                                    else {
                                        return false;
                                    }
                                    if(sql[outptr] == ")") {
                                        outptr++;
                                        return true;
                                    }
                                    if(sql[outptr] == ",") {
                                        outptr++;
                                    }
                                }
                            }
                        }
                        else {
                            return false;
                        }
                    }
                    return false;
                }
                else {
                    return false;
                }
            }
            else if(sql[outptr].ToUpper() == "JOIN") {
                s.joinType += sql[outptr].ToUpper();
                outptr++;
                if (TableReference(sql,outptr,out int x,out SourceItem y)) {
                    outptr = x;
                    s.schema = y.schema;
                    s.table = y.table;
                    s.query = y.query;
                    s.alias = y.alias;
                    if(sql[outptr].ToUpper() == "ON") {
                        outptr++;
                        if (Condition(sql,outptr,out int x2,out IConditionItem y2)) {
                            outptr = x2;
                            s.joinCondition = y2;
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    else if(sql[outptr].ToUpper() == "USING") {
                        outptr++;
                        if(sql[outptr] == "(") {
                            outptr++;
                            while(true) {
                                if (IsValidIdentifier(sql[outptr])) {
                                    s.joinOnColumns.Add(sql[outptr]);
                                    outptr++;
                                }
                                else {
                                    return false;
                                }
                                if(sql[outptr] == ")") {
                                    outptr++;
                                    return true;
                                }
                                if(sql[outptr] == ",") {
                                    outptr++;
                                }
                            }
                        }
                    }
                    else {
                        return false;
                    }
                }
                return false;
            }
            else if(sql[outptr].ToUpper() == "CROSS") {
                s.joinType += sql[outptr].ToUpper();
                outptr++;
                if(sql[outptr].ToUpper() == "JOIN") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (TableReference(sql,outptr,out int x,out SourceItem y)) {
                    outptr = x;
                    s.schema = y.schema;
                    s.table = y.table;
                    s.query = y.query;
                    s.alias = y.alias;
                    return true;
                }
                return false;
            }
            else if(sql[outptr].ToUpper() == "NATURAL") {
                s.joinType += sql[outptr].ToUpper();
                outptr++;
                if(sql[outptr].ToUpper() == "INNER") {
                    s.joinType += sql[outptr].ToUpper();
                    outptr++;
                }
                if(sql[outptr].ToUpper() == "JOIN") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (TableReference(sql,outptr,out int x,out SourceItem y)) {
                    outptr = x;
                    s.schema = y.schema;
                    s.table = y.table;
                    s.query = y.query;
                    s.alias = y.alias;
                    return true;
                }
                return false;
            }
            else {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    private bool OuterJoinClause(List<string> sql, int inptr, out int outptr, out SourceItem s)
    {
        outptr = inptr;
        s = new SourceItem();
        try
        {
            if(sql[outptr].ToUpper() == "NATURAL") {
                s.joinType += "NATURAL";
                outptr++;
            }
            if(sql[outptr].ToUpper() == "FULL" || sql[outptr].ToUpper() == "LEFT" || sql[outptr].ToUpper() == "RIGHT") {
                s.joinType += sql[outptr].ToUpper();
                outptr++;
            }
            else {
                return false;
            }
            if(sql[outptr].ToUpper() == "OUTER") {
                s.joinType += sql[outptr].ToUpper();
                outptr++;
            }
            if(sql[outptr].ToUpper() == "JOIN") {
                outptr++;
            }
            else {
                return false;
            }
            if (TableReference(sql,outptr,out int x,out SourceItem y)) {
                outptr = x;
                s.schema = y.schema;
                s.table = y.table;
                s.query = y.query;
                s.alias = y.alias;
                if(sql[outptr].ToUpper() == "ON") {
                    outptr++;
                    if (Condition(sql,outptr,out int x2,out IConditionItem y2)) {
                        outptr = x2;
                        s.joinCondition = y2;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else if(sql[outptr].ToUpper() == "USING") {
                    outptr++;
                    if(sql[outptr] == "(") {
                        outptr++;
                        while(true) {
                            if (IsValidIdentifier(sql[outptr])) {
                                s.joinOnColumns.Add(sql[outptr]);
                                outptr++;
                            }
                            else {
                                return false;
                            }
                            if(sql[outptr] == ")") {
                                outptr++;
                                return true;
                            }
                            if(sql[outptr] == ",") {
                                outptr++;
                            }
                        }
                    }
                }
                else {
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool WhereClause(List<string> sql, int inptr, out int outptr, out IConditionItem c)
    {
        outptr = inptr;
        ConditionItem ci = new ConditionItem();
        c = ci;
        try
        {
            if(sql[outptr].ToUpper() =="WHERE") {
                outptr++;
            }
            else {
                return false;
            }
            if (Condition(sql,outptr,out int x,out IConditionItem y)) {
                outptr = x;
                c = y;
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool GroupByClause(List<string> sql, int inptr, out int outptr, out List<IExpressionItem> el, out IConditionItem c)
    {
        outptr = inptr;
        ConditionItem ci = new ConditionItem();
        c = ci;
        el = [];
        try
        {
            if(sql[outptr].ToUpper() == "GROUP" && sql[outptr+1].ToUpper() == "BY") {
                outptr+=2;
            }
            else {
                return false;
            }
            while(true) {
                if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                    outptr = x;
                    el.Add(y);
                }
                else {
                    return false;
                }
                if(sql[outptr] == ",") {
                    outptr++;
                }
                else {
                    break;
                }
            }
            if(sql[outptr].ToUpper() == "HAVING") {
                outptr++;
                if (Condition(sql,outptr,out int x2,out IConditionItem y2)) {
                    outptr = x2;
                    c = y2;
                    return true;
                }
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool OrderByClause(List<string> sql, int inptr, out int outptr, out string orderByModifier, out List<OrderByItem> orderByItems)
    {
        outptr = inptr;
        orderByModifier = "";
        orderByItems = [];
        try
        {
            if(sql[outptr].ToUpper() == "ORDER") {
                outptr++;
            }
            else {
                return false;
            }
            if(sql[outptr].ToUpper() == "SIBLINGS") {
                orderByModifier = "SIBLINGS";
                outptr++;
            }
            if(sql[outptr].ToUpper() == "BY") {
                outptr++;
            }
            else {
                return false;
            }
            while(true) {
                OrderByItem o = new OrderByItem();
                if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                    outptr = x;
                    o.expression = y;
                    if(sql[outptr].ToUpper() == "ASC" || sql[outptr].ToUpper() == "DESC") {
                        o.direction = sql[outptr].ToUpper();
                        outptr++;
                    }
                    if((sql[outptr].ToUpper() == "NULLS" && sql[outptr+1].ToUpper() == "FIRST") || (sql[outptr].ToUpper() == "NULLS" && sql[outptr+1].ToUpper() == "LAST")) {
                        o.nulls = sql[outptr+1].ToUpper();
                        outptr+=2;
                    }
                    orderByItems.Add(o);
                }
                else {
                    return false;
                }
                if(sql[outptr] == ",") {
                    outptr++;
                }
                else {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }
    private bool RowLimitingClause(List<string> sql, int inptr, out int outptr, out string s)
    {
        outptr = inptr;
        s = "";
        try
        {
            if(sql[outptr].ToUpper() == "OFFSET") {
                s += sql[outptr].ToUpper();
                outptr++;
                if(long.TryParse(sql[outptr],out _)) {
                    s += " " + sql[outptr];
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr].ToUpper() == "ROW" || sql[outptr].ToUpper() == "ROWS") {
                    s += " " + sql[outptr].ToUpper();
                    outptr++;
                }
                else {
                    return false;
                }
            }
            if(sql[outptr].ToUpper() == "FETCH") {
                s += " " + sql[outptr].ToUpper();
                outptr++;
                if(sql[outptr].ToUpper() == "FIRST" || sql[outptr].ToUpper() == "NEXT") {
                    s += " " + sql[outptr].ToUpper();
                    outptr++;
                }
                else {
                    return false;
                }
                if(long.TryParse(sql[outptr],out _)) {
                    s += " " + sql[outptr].ToUpper();
                    outptr++;
                    if(sql[outptr].ToUpper() == "PERCENT") {
                        s += " " + sql[outptr].ToUpper();
                        outptr++;
                    }
                }
                if(sql[outptr].ToUpper() == "ROW" || sql[outptr].ToUpper() == "ROWS") {
                    s += " " + sql[outptr].ToUpper();
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr].ToUpper() == "ONLY") {
                    s += " " + sql[outptr].ToUpper();
                    outptr++;
                }
                else if(sql[outptr].ToUpper() == "WITH" && sql[outptr+1].ToUpper() == "TIES") {
                    s += " " +sql[outptr].ToUpper() + " " + sql[outptr+1].ToUpper();
                    outptr+=2;
                }
                else {
                    return false;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool Condition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        ConditionItem ci = new ConditionItem();
        e = ci;
        try
        {
            while (true) {
                if (SimpleComparisonCondition(sql,outptr,out int x,out IConditionItem y)) {
                    outptr = x;
                    ci.conditionItems.Add(y);
                }
                else if (GroupComparisonCondition(sql,outptr,out int x2,out IConditionItem y2)) {
                    outptr = x2;
                    ci.conditionItems.Add(y2);
                }
                else if (FloatingPointCondition(sql,outptr,out int x3,out IConditionItem y3)) {
                    outptr = x3;
                    ci.conditionItems.Add(y3);
                }
                else if (CompoundCondition(sql,outptr,out int x4,out IConditionItem y4)) {
                    outptr = x4;
                    ci.conditionItems.Add(y4);
                }
                else if (LikeCondition(sql,outptr,out int x5,out IConditionItem y5)) {
                    outptr = x5;
                    ci.conditionItems.Add(y5);
                }
                else if (RegexLikeCondition(sql,outptr,out int x6,out IConditionItem y6)) {
                    outptr = x6;
                    ci.conditionItems.Add(y6);
                }
                else if (NullCondition(sql,outptr,out int x7,out IConditionItem y7)) {
                    outptr = x7;
                    ci.conditionItems.Add(y7);
                }
                else if (BetweenCondition(sql,outptr,out int x8,out IConditionItem y8)) {
                    outptr = x8;
                    ci.conditionItems.Add(y8);
                }
                else if (ExistsCondition(sql,outptr,out int x9,out IConditionItem y9)) {
                    outptr = x9;
                    ci.conditionItems.Add(y9);
                }
                else if (InCondition(sql,outptr,out int x10,out IConditionItem y10)) {
                    outptr = x10;
                    ci.conditionItems.Add(y10);
                }
                else {
                    return false;
                }
                if(sql[outptr].ToUpper() == "AND" || sql[outptr].ToUpper() == "OR") {
                    LogicalConditionItem lc = new LogicalConditionItem();
                    lc.logicalCondition = sql[outptr].ToUpper();
                    ci.conditionItems.Add(lc);
                    outptr++;
                }
                else {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }
    private bool SimpleComparisonCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        SimpleComparisonConditionItem scc = new SimpleComparisonConditionItem();
        e = scc;
        try
        {
            if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                outptr = x;
                scc.leftExpressionItems.Add(y);
                if(sql[outptr] == "=" || sql[outptr] == "!=" || sql[outptr] == "^=" || sql[outptr] == "<>" || sql[outptr] == ">" || sql[outptr] == "<" || sql[outptr] == ">=" || sql[outptr] == "<=") {
                    scc.comparisonOperator = sql[outptr];
                    outptr++;
                }
                else {
                    return false;
                }
                if (Expression(sql,outptr,out int x2,out IExpressionItem y2)) {
                    outptr = x2;
                    scc.rightExpressionItems.Add(y2);
                    return true;
                }
                else {
                    return false;
                }
            }
            else if(sql[outptr] == "(") {
                outptr++;
                while(true) {
                    if (Expression(sql,outptr,out int x3,out IExpressionItem y3)) {
                        outptr = x3;
                        scc.leftExpressionItems.Add(y3);
                    }
                    else {
                        return false;
                    }
                    if(sql[outptr] == ")") {
                        outptr++;
                        break;
                    }
                    if(sql[outptr] == ",") {
                        outptr++;
                    }
                }
                if(sql[outptr] == "=" || sql[outptr] == "!=" || sql[outptr] == "^=" || sql[outptr] == "<>") {
                    scc.comparisonOperator = sql[outptr];
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr] == "(") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (SubQuery(sql,outptr,out int x4,out Query y4)) {
                    outptr = x4;
                    scc.subquery = y4;
                    if (sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else if (ExpressionList(sql,outptr,out int x5,out List<IExpressionItem> y5)) {
                    outptr = x5;
                    scc.rightExpressionItems = y5;
                    if (sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    private bool GroupComparisonCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        GroupComparisonConditionItem gcc = new GroupComparisonConditionItem();
        e = gcc;
        try
        {
            if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                outptr = x;
                gcc.leftExpressionItems.Add(y);
                if(sql[outptr] == "=" || sql[outptr] == "!=" || sql[outptr] == "^=" || sql[outptr] == "<>" || sql[outptr] == ">" || sql[outptr] == "<" || sql[outptr] == ">=" || sql[outptr] == "<=") {
                    gcc.comparisonOperator = sql[outptr];
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr].ToUpper() == "ANY" || sql[outptr].ToUpper() == "SOME" || sql[outptr].ToUpper() == "ALL") {
                    gcc.groupModifier = sql[outptr].ToUpper();
                }
                else {
                    return false;
                }
                if(sql[outptr] == "(") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (SubQuery(sql,outptr,out int x2,out Query y2)) {
                    outptr = x2;
                    gcc.subquery = y2;
                    if(sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else if (ExpressionList(sql,outptr,out int x3,out List<IExpressionItem> y3)) {
                    outptr = x3;
                    gcc.rightExpressionItems.Add(y3);
                    if (sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
            else if(sql[outptr] == "(") {
                outptr++;
                while(true) {
                    if (Expression(sql,outptr,out int x4,out IExpressionItem y4)) {
                        outptr = x4;
                        gcc.leftExpressionItems.Add(y4);
                    }
                    else {
                        return false;
                    }
                    if(sql[outptr] == ")") {
                        outptr++;
                        break;
                    }
                    if(sql[outptr] == ",") {
                        outptr++;
                    }
                }
                if(sql[outptr] == "=" || sql[outptr] == "!=" || sql[outptr] == "^=" || sql[outptr] == "<>") {
                    gcc.comparisonOperator = sql[outptr];
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr].ToUpper() == "ANY" || sql[outptr].ToUpper() == "SOME" || sql[outptr].ToUpper() == "ALL") {
                    gcc.groupModifier = sql[outptr].ToUpper();
                }
                else {
                    return false;
                }
                if(sql[outptr] == "(") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (SubQuery(sql,outptr,out int x5,out Query y5)) {
                    outptr = x5;
                    gcc.subquery = y5;
                    if(sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    while(true) {
                        if (ExpressionList(sql,outptr,out int x6,out List<IExpressionItem> y6)) {
                            outptr = x6;
                            gcc.rightExpressionItems.Add(y6);
                        }
                        else {
                            return false;
                        }
                        if (sql[outptr] == ")") {
                            outptr++;
                            return true;
                        }
                        else if (sql[outptr] == ",") {
                            outptr++;
                        }
                        else {
                            return false;
                        }
                    }
                }
            }
            else {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    private bool FloatingPointCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        FloatingPointConditionItem fpc = new FloatingPointConditionItem();
        e = fpc;
        try
        {
            if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                outptr = x;
                fpc.conditionExpression = y;
                if(sql[outptr].ToUpper() == "IS") {
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr].ToUpper() == "NOT") {
                    fpc.value = "NOT";
                    outptr++;
                }
                if(sql[outptr].ToUpper() == "NAN" || sql[outptr].ToUpper() == "INFINITE") {
                    fpc.value += sql[outptr].ToUpper();
                    outptr++;
                    return true;
                }
                else {
                    return false;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool CompoundCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        CompoundConditionItem cc = new CompoundConditionItem();
        e = cc;
        try
        {
            if(sql[outptr] == "(") {
                outptr++;
                if (Condition(sql,outptr,out int x,out IConditionItem y)) {
                    outptr = x;
                    cc.condition = y;
                    if(sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                return false;
            }
            else if(sql[outptr].ToUpper() == "NOT") {
                cc.logic = "NOT";
                outptr++;
                if (Condition(sql,outptr,out int x2,out IConditionItem y2)) {
                    outptr = x2;
                    cc.condition = y2;
                    return true;
                }
                return false;
            }
            else {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    private bool LikeCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        LikeConditionItem lc = new LikeConditionItem();
        e = lc;
        try
        {
            lc.likeCondition = sql[outptr];
            outptr++;
            if(sql[outptr].ToUpper() == "NOT") {
                lc.likeCondition += "NOT";
                outptr++;
            }
            if(sql[outptr] == "LIKE" || sql[outptr] == "LIKEC" || sql[outptr] == "LIKE2" || sql[outptr] == "LIKE4") {
                lc.likeCondition += sql[outptr].ToUpper();
                outptr++;
            }
            else {
                return false;
            }
            lc.likeCondition += sql[outptr];
            outptr++;
            if(sql[outptr].ToUpper() == "ESCAPE") {
                lc.likeCondition += "ESCAPE";
                outptr++;
                lc.likeCondition += sql[outptr];
                outptr++;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool RegexLikeCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        RegexLikeConditionItem rlc = new RegexLikeConditionItem();
        e = rlc;
        try
        {
            if(sql[outptr].ToUpper() == "REGEXP_LIKE") {
                outptr++;
            }
            else {
                return false;
            }
            if(sql[outptr] == "(") {
                outptr++;
            }
            else {
                return false;
            }
            rlc.regexLikeCondition = sql[outptr];
            outptr++;
            if(sql[outptr] == ",") {
                outptr++;
            }
            else {
                return false;
            }
            rlc.regexLikeCondition += sql[outptr];
            outptr++;
            if(sql[outptr] == ",") {
                outptr++;
                rlc.regexLikeCondition += sql[outptr];
            }
            if(sql[outptr] == ")") {
                outptr++;
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool NullCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        NullConditionItem nc = new NullConditionItem();
        e = nc;
        try
        {
            if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                outptr = x;
                nc.conditionExpression = y;
                if(sql[outptr].ToUpper() == "IS") {
                    outptr++;
                    if(sql[outptr].ToUpper() == "NOT") {
                        nc.nullCondition = "NOT";
                        outptr++;
                    }
                    if(sql[outptr].ToUpper() == "NULL") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool BetweenCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        BetweenConditionItem bc = new BetweenConditionItem();
        e = bc;
        try
        {
            if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                outptr = x;
                bc.expression1 = y;
                if(sql[outptr].ToUpper() == "NOT") {
                    bc.logic = "NOT";
                    outptr++;
                }
                if(sql[outptr].ToUpper() == "BETWEEN") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (Expression(sql,outptr,out int x2,out IExpressionItem y2)) {
                    outptr = x2;
                    bc.expression2 = y2;
                    if(sql[outptr].ToUpper() == "AND") {
                        outptr++;
                    }
                    else {
                        return false;
                    }
                    if (Expression(sql,outptr,out int x3,out IExpressionItem y3)) {
                        outptr = x3;
                        bc.expression3 = y3;
                        return true;
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool ExistsCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        ExistsConditionItem ec = new ExistsConditionItem();
        e = ec;
        try
        {
            if(sql[outptr].ToUpper() == "EXISTS") {
                outptr++;
            }
            else {
                return false;
            }
            if(sql[outptr] == "(") {
                outptr++;
            }
            else {
                return false;
            }
            if (SubQuery(sql,outptr,out int x,out Query y)) {
                outptr = x;
                ec.subQuery = y;
                if(sql[outptr] == ")") {
                    outptr++;
                    return true;
                }
                return false;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool InCondition(List<string> sql, int inptr, out int outptr, out IConditionItem e)
    {
        outptr = inptr;
        InConditionItem ic = new InConditionItem();
        e = ic;
        try
        {
            if(sql[outptr] == "(") {
                outptr++;
                while(true) {
                    if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                        outptr = x;
                        ic.leftExpressionItem.Add(y);
                    }
                    else {
                        return false;
                    }
                    if(sql[outptr] == ")") {
                        outptr++;
                        break;
                    }
                    if(sql[outptr] == ",") {
                        outptr++;
                    }
                    else {
                        return false;
                    }
                }
                if(sql[outptr].ToUpper() == "NOT") {
                    ic.logic = "NOT";
                    outptr++;
                }
                if(sql[outptr].ToUpper() == "IN") {
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr] == "(") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (SubQuery(sql,outptr,out int x2,out Query y2)) {
                    outptr = x2;
                    ic.subQuery = y2;
                    if(sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    while(true) {
                        if (ExpressionList(sql,outptr,out int x3,out List<IExpressionItem> y3)) {
                            ic.rightExpressionItems.Add(y3);
                        }
                        else {
                            return false;
                        }
                        if(sql[outptr] == ")") {
                            outptr++;
                            return true;
                        }
                        if(sql[outptr] == ",") {
                            outptr++;
                        }
                        else {
                            return false;
                        }
                    }
                }
            }
            else if (Expression(sql,outptr,out int x4,out IExpressionItem y4)) {
                outptr = x4;
                ic.leftExpressionItem.Add(y4);
                if(sql[outptr].ToUpper() == "NOT") {
                    ic.logic = "NOT";
                    outptr++;
                }
                if(sql[outptr].ToUpper() == "IN") {
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr] == "(") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (SubQuery(sql,outptr,out int x5,out Query y5)) {
                    outptr = x5;
                    ic.subQuery = y5;
                    if(sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else if (ExpressionList(sql,outptr,out int x6,out List<IExpressionItem> y6)) {
                    outptr = x6;
                    ic.rightExpressionItems.Add(y6);
                    if(sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool Expression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        ExpressionItem ei = new ExpressionItem();
        e = ei;
        try
        {
            while(true) {
                if (CaseExpression(sql,outptr,out int x3,out IExpressionItem y3)) {
                    outptr = x3;
                    ei.expressionItems.Add(y3);
                }
                else if (SimpleExpression(sql,outptr,out int x,out IExpressionItem y)) {
                    outptr = x;
                    ei.expressionItems.Add(y);
                }
                else if (CompoundExpression(sql,outptr,out int x2,out IExpressionItem y2)) {
                    outptr = x2;
                    ei.expressionItems.Add(y2);
                }
                else if (FunctionExpression(sql,outptr,out int x4,out IExpressionItem y4)) {
                    outptr = x4;
                    ei.expressionItems.Add(y4);
                }
                else if (ScalarSubQueryExpression(sql,outptr,out int x5,out IExpressionItem y5)) {
                    outptr = x5;
                    ei.expressionItems.Add(y5);
                }
                else {
                    return false;
                }
                if (sql[outptr] == "*" || sql[outptr] == "/" || sql[outptr] == "+" || sql[outptr] == "-" || sql[outptr] == "||") {
                    OperatorExpressionItem oe = new OperatorExpressionItem();
                    oe.expressionOperator = sql[outptr];
                    ei.expressionItems.Add(oe);
                    outptr++;
                }
                else {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }
    private bool ExpressionList(List<string> sql, int inptr, out int outptr, out List<IExpressionItem> el)
    {
        outptr = inptr;
        el = [];
        try
        {
            if(sql[outptr] == "(") {
                outptr++;
                while(true) {
                    if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                        outptr = x;
                        el.Add(y);
                    }
                    else {
                        return false;
                    }
                    if (sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                    if (sql[outptr] == ",") {
                        outptr++;
                    }
                    else {
                        return false;
                    }
                }
            }
            else {
                while(true) {
                    if (Expression(sql,outptr,out int x2,out IExpressionItem y2)) {
                        outptr = x2;
                        el.Add(y2);
                    }
                    else {
                        return false;
                    }
                    if (sql[outptr] == ",") {
                        outptr++;
                    }
                    else {
                        return false;
                    }
                }
            }
        }
        catch
        {
            return false;
        }
    }
    private bool SimpleExpression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        SimpleExpressionItem se = new SimpleExpressionItem();
        e = se;
        try
        {
            if (sql[outptr].ToUpper() == "ROWNUM" || sql[outptr].ToUpper() == "ROWID" || sql[outptr].ToUpper() == "NULL") {
                se.field = sql[outptr].ToUpper();
                outptr++;
                return true;
            }
            if (sql[outptr] != "*" && sql[outptr] != "," && sql[outptr] != "." && sql[outptr] != "(" && sql[outptr] != ")" && !IsReserved(sql[outptr]) && !IsFunction(sql[outptr])) {
                se.field = sql[outptr];
                outptr++;
            }
            else {
                return false;
            }
            if(sql[outptr] == ".") {
                outptr++;
                if(IsValidTable(se.field) && (IsValidIdentifier(sql[outptr]) || sql[outptr].ToUpper() == "ROWID")) {
                    se.table = se.field;
                    se.field = sql[outptr];
                    outptr++;
                }
                else {
                    return false;
                }
                if(sql[outptr] == ".") {
                    outptr++;
                    if(IsValidTable(se.field) && (IsValidIdentifier(sql[outptr]) || sql[outptr].ToUpper() == "ROWID")) {
                        se.schema = se.table;
                        se.table = se.field;
                        se.field = sql[outptr];
                        outptr++;
                    }
                    else {
                        return false;
                    }
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool CompoundExpression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        CompoundExpressionItem ce = new CompoundExpressionItem();
        e = ce;
        try
        {
            if(sql[outptr] == "(") {
                outptr++;
                if (Expression(sql,outptr,out int x, out IExpressionItem y)) {
                    outptr = x;
                    ce.expression = y;
                    if (sql[outptr] == ")") {
                        outptr++;
                        return true;
                    }
                }
                return false;
            }
            if(sql[outptr] == "+" || sql[outptr] == "-" || sql[outptr].ToUpper() == "PRIOR") {
                ce.expressionOperator = sql[outptr].ToUpper();
                outptr++;
                if (Expression(sql,outptr,out int x, out IExpressionItem y)) {
                    outptr = x;
                    ce.expression = y;
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool CaseExpression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        CaseExpressionItem cse = new CaseExpressionItem();
        e = cse;
        try
        {
            if (sql[outptr].ToUpper() == "CASE") {
                outptr++;
            }
            else {
                return false;
            }
            if (SimpleCaseExpression(sql,outptr,out int x,out IExpressionItem y)) {
                outptr = x;
                cse.caseExpression = y;
            }
            else if (SearchedCaseExpression(sql,outptr,out int x2,out IExpressionItem y2)) {
                outptr = x2;
                cse.caseExpression = y2;
            }
            else {
                return false;
            }
            if (sql[outptr].ToUpper() == "ELSE") {
                outptr++;
                if (Expression(sql,outptr,out int x3,out IExpressionItem y3)) {
                    outptr = x3;
                    cse.elseExpression = y3;
                }
                else {
                    return false;
                }
            }
            if (sql[outptr].ToUpper() == "END") {
                outptr++;
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool SimpleCaseExpression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        SimpleCaseExpressionItem sce = new SimpleCaseExpressionItem();
        e = sce;
        try
        {
            if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                outptr = x;
                sce.expression = y;
                while(true) {
                    if(sql[outptr].ToUpper() == "WHEN") {
                        outptr++;
                    }
                    else {
                        return false;
                    }
                    if (Expression(sql,outptr,out int x2,out IExpressionItem y2)) {
                        outptr = x2;
                        if (sql[outptr].ToUpper() == "THEN") {
                            outptr++;
                        }
                        else {
                            return false;
                        }
                        if (Expression(sql,outptr,out int x3,out IExpressionItem y3)) {
                            outptr = x3;
                            sce.pairs.Add((y2,y3));
                            if (sql[outptr].ToUpper() != "WHEN") {
                                return true;
                            }
                        }
                        else {
                            return false;
                        }
                    }
                    return false;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool SearchedCaseExpression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        SearchedCaseExpressionItem srce = new SearchedCaseExpressionItem();
        e = srce;
        try
        {
            while(true) {
                if(sql[outptr].ToUpper() == "WHEN") {
                    outptr++;
                }
                else {
                    return false;
                }
                if (Condition(sql,outptr,out int x,out IConditionItem y)) {
                    outptr = x;
                    if (sql[outptr].ToUpper() == "THEN") {
                        outptr++;
                    }
                    else {
                        return false;
                    }
                    if (Expression(sql,outptr,out int x2,out IExpressionItem y2)) {
                        outptr = x2;
                        srce.pairs.Add((y,y2));
                        if (sql[outptr].ToUpper() != "WHEN") {
                            return true;
                        }
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
        }
        catch
        {
            return false;
        }
    }
    private bool FunctionExpression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        FunctionExpressionItem fe = new FunctionExpressionItem();
        e = fe;
        try
        {
            if (IsFunction(sql[outptr])) {
                fe.function = sql[outptr].ToUpper();
                outptr++;
                if (sql[outptr] == "(") {
                    outptr++;
                    while (true) {
                        if (Expression(sql,outptr,out int x,out IExpressionItem y)) {
                            outptr = x;
                            fe.parameters.Add(y);
                        }
                        else {
                            return false;
                        }
                        if (sql[outptr] == ")") {
                            outptr++;
                            return true;
                        }
                        if (sql[outptr] == ",") {
                            outptr++;
                        }
                        else {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool ScalarSubQueryExpression(List<string> sql, int inptr, out int outptr, out IExpressionItem e)
    {
        outptr = inptr;
        ScalarSubQueryExpressionItem sqe = new ScalarSubQueryExpressionItem();
        e = sqe;
        try
        {
            if(sql[outptr] == "(") {
                List<string> temp = [];
                outptr++;
                int parenthesesDepth = 1;
                while(true) {
                    if(sql[outptr] == "(") {
                        parenthesesDepth++;
                    }
                    else if(sql[outptr] == ")") {
                        parenthesesDepth--;
                        if(parenthesesDepth == 0) {
                            temp.Add("#EOF");
                            outptr++;
                            if (SubQuery(temp,0,out _, out Query y)) {
                                sqe.subQuery = y;
                                return true;
                            }
                            return false;
                        }
                    }
                    temp.Add(sql[outptr]);
                    outptr++;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}