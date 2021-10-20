using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Clauses
{
    public class JoinClause
    {
        public ClauseType JoinType;
        public string FromTable;
        public string FromColumn;
        public Comparison ComparisonOperator;
        public string ToTable;
        public string ToColumn;

        public JoinClause(ClauseType join, string toTableName, string toColumnName, Comparison operation, string fromTableName, string fromColumnName)
        {
            JoinType = join;
            FromTable = fromTableName;
            FromColumn = fromColumnName;
            ComparisonOperator = operation;
            ToTable = toTableName;
            ToColumn = toColumnName;
        }
    }
}
