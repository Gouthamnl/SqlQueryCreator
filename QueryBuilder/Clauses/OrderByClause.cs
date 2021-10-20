using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Clauses
{
    public class OrderByClause
    {
        public string FieldName;
        public Sort SortOrder;
        public OrderByClause(string field)
        {
            FieldName = field;
            SortOrder = Sort.Ascending;
        }
        public OrderByClause(string field, Sort order)
        {
            FieldName = field;
            SortOrder = order;
        }
    }
}
