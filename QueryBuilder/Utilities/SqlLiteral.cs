using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Utilities
{
    public class SqlLiteral
    {
        public static string StatementRowsAffected = "SELECT @@ROWCOUNT";

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public SqlLiteral(string value)
        {
            _value = value;
        }
    }
}
