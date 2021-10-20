using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Enums
{
    public enum ClauseType
    {
        Where,
        InnerJoin,
        OuterJoin,
        LeftJoin,
        RightJoin,
        OrderBy
    }
}
