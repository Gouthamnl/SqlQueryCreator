using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Clauses
{
    public class WhereClause
    {
        internal class SubClause
        {
            public LogicOperator LogicOperator;
            public Comparison ComparisonOperator;
            public object Value;
            public SubClause(LogicOperator logic, Comparison compareOperator, object compareValue)
            {
                LogicOperator = logic;
                ComparisonOperator = compareOperator;
                Value = compareValue;
            }
        }
        internal List<SubClause> SubClauses;	// Array of SubClause

        /// <summary>
        /// Gets/sets the name of the database column this WHERE clause should operate on
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets/sets the comparison method
        /// </summary>
        public Comparison ComparisonOperator { get; set; }

        /// <summary>
        /// Gets/sets the value that was set for comparison
        /// </summary>
        public object Value { get; set; }

        public WhereClause(string field, Comparison firstCompareOperator, object firstCompareValue)
        {
            FieldName = field;
            ComparisonOperator = firstCompareOperator;
            Value = firstCompareValue;
            SubClauses = new List<SubClause>();
        }
        public void AddClause(LogicOperator logic, Comparison compareOperator, object compareValue)
        {
            SubClause NewSubClause = new SubClause(logic, compareOperator, compareValue);
            SubClauses.Add(NewSubClause);
        }
    }
}
