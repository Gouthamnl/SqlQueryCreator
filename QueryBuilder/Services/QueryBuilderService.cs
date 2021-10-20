using QueryBuilder.Clauses;
using QueryBuilder.Enums;
using QueryBuilder.Interface;
using QueryBuilder.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace QueryBuilder.Services
{
    public class QueryBuilderService : IQueryBuilderService
    {
        private readonly List<string> _selectedColumns;
        private readonly List<string> _selectedTables;	
        private readonly List<JoinClause> _joins;
        private readonly List<string> _groupByColumns;
        private readonly List<OrderByClause> _orderByStatement;


        /// <summary>
        /// General constructor : Initialize the readonly fields
        /// </summary>
        public QueryBuilderService() 
        {
            _selectedColumns = new List<string>();
            _selectedTables = new List<string>();
            _joins = new List<JoinClause>();
            _groupByColumns = new List<string>();
            _orderByStatement = new List<OrderByClause>();
        }
        public QueryBuilderService(DbProviderFactory factory)
        {
            _dbProviderFactory = factory;
        }

        private DbProviderFactory _dbProviderFactory;
        public void SetDbProviderFactory(DbProviderFactory factory)
        {
            _dbProviderFactory = factory;
        }

        public bool Distinct { get; set; } = false;

        public string[] SelectedColumns
        {
            get
            {
                if (_selectedColumns.Count > 0)
                    return _selectedColumns.ToArray();
                else
                    return new string[1] { "*" };
            }
        }
        public string[] SelectedTables
        {
            get { return _selectedTables.ToArray(); }
        }

        public void SelectAllColumns()
        {
            _selectedColumns.Clear();
        }
        public void SelectCount()
        {
            SelectColumn("count(1)");
        }
        public void SelectColumn(string column)
        {
            _selectedColumns.Clear();
            _selectedColumns.Add(column);
        }
        public void SelectColumns(params string[] columns)
        {
            _selectedColumns.Clear();
            foreach (string column in columns)
            {
                _selectedColumns.Add(column);
            }
        }
        public void SelectFromTable(string table)
        {
            _selectedTables.Clear();
            _selectedTables.Add(table);
        }
        public void SelectFromTables(params string[] tables)
        {
            _selectedTables.Clear();
            foreach (string Table in tables)
            {
                _selectedTables.Add(Table);
            }
        }
        public void AddJoin(JoinClause newJoin)
        {
            _joins.Add(newJoin);
        }
        public void AddJoin(ClauseType join, string toTableName, string toColumnName, Comparison operation, string fromTableName, string fromColumnName)
        {
            JoinClause NewJoin = new JoinClause(join, toTableName, toColumnName, operation, fromTableName, fromColumnName);
            _joins.Add(NewJoin);
        }

        public WhereStatement Where { get; set; } = new WhereStatement();

        public void AddWhere(WhereClause clause) { AddWhere(clause, 1); }
        public void AddWhere(WhereClause clause, int level)
        {
            Where.Add(clause, level);
        }
        public WhereClause AddWhere(string field, Comparison operation, object compareValue) { return AddWhere(field, operation, compareValue, 1); }
        public WhereClause AddWhere(Enum field, Comparison operation, object compareValue) { return AddWhere(field.ToString(), operation, compareValue, 1); }
        public WhereClause AddWhere(string field, Comparison operation, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, operation, compareValue);
            Where.Add(NewWhereClause, level);
            return NewWhereClause;
        }

        public void AddOrderBy(OrderByClause clause)
        {
            _orderByStatement.Add(clause);
        }
        public void AddOrderBy(Enum field, Sort order) { this.AddOrderBy(field.ToString(), order); }
        public void AddOrderBy(string field, Sort order)
        {
            OrderByClause NewOrderByClause = new OrderByClause(field, order);
            _orderByStatement.Add(NewOrderByClause);
        }

        public void GroupBy(params string[] columns)
        {
            foreach (string Column in columns)
            {
                _groupByColumns.Add(Column);
            }
        }

        public WhereStatement Having { get; set; } = new WhereStatement();

        public void AddHaving(WhereClause clause) { AddHaving(clause, 1); }
        public void AddHaving(WhereClause clause, int level)
        {
            Having.Add(clause, level);
        }
        public WhereClause AddHaving(string field, Comparison operation, object compareValue) { return AddHaving(field, operation, compareValue, 1); }
        public WhereClause AddHaving(Enum field, Comparison operation, object compareValue) { return AddHaving(field.ToString(), operation, compareValue, 1); }
        public WhereClause AddHaving(string field, Comparison operation, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, operation, compareValue);
            Having.Add(NewWhereClause, level);
            return NewWhereClause;
        }

        public DbCommand BuildCommand()
        {
            return (DbCommand)this.BuildQuery(true);
        }

        public string BuildQuery()
        {
            return (string)this.BuildQuery(false);
        }

        /// <summary>
        /// Builds the select query
        /// </summary>
        /// <returns>Returns a string containing the query, or a DbCommand containing a command with parameters</returns>
        private object BuildQuery(bool buildCommand)
        {
            if (buildCommand && _dbProviderFactory == null)
                throw new Exception("Cannot build a command when the Db Factory is not specified.");

            DbCommand command = null;
            if (buildCommand)
                command = _dbProviderFactory.CreateCommand();

            string Query = "SELECT ";

            // Output Distinct
            if (Distinct)
            {
                Query += "DISTINCT ";
            }

            // Output column names
            if (_selectedColumns.Count == 0)
            {
                if (_selectedTables.Count > 1)
                    Query += _selectedTables[0] + "."; // By default only select * from the table that was selected. If there are any joins, it is the responsibility of the user to select the needed columns.

                Query += "*";
            }
            else
            {
                foreach (string ColumnName in _selectedColumns)
                {
                    Query += ' ' + ColumnName + ',';
                }
                Query = Query.TrimEnd(','); // Trim the last comma inserted by foreach loop
                Query += ' ';
            }
            // Output table names
            if (_selectedTables.Count > 0)
            {
                Query += " FROM ";
                foreach (string TableName in _selectedTables)
                {
                    Query += TableName + ',';
                }
                Query = Query.TrimEnd(','); // Trim the last comma inserted by foreach loop
                Query += ' ';
            }

            // Output joins
            if (_joins.Count > 0)
            {
                foreach (JoinClause Clause in _joins)
                {
                    string JoinString = "";
                    switch (Clause.JoinType)
                    {
                        case ClauseType.InnerJoin: JoinString = "INNER JOIN"; break;
                        case ClauseType.OuterJoin: JoinString = "OUTER JOIN"; break;
                        case ClauseType.LeftJoin: JoinString = "LEFT JOIN"; break;
                        case ClauseType.RightJoin: JoinString = "RIGHT JOIN"; break;
                    }
                    JoinString += " " + Clause.ToTable + " ON ";
                    JoinString += WhereStatement.CreateComparisonClause(Clause.FromTable + '.' + Clause.FromColumn, Clause.ComparisonOperator, new SqlLiteral(Clause.ToTable + '.' + Clause.ToColumn));
                    Query += JoinString + ' ';
                }
            }

            // Output where statement
            if (Where.ClauseLevels > 0)
            {
                if (buildCommand)
                    Query += " WHERE " + Where.BuildWhereStatement(true, ref command);
                else
                    Query += " WHERE " + Where.BuildWhereStatement();
            }

            // Output GroupBy statement
            if (_groupByColumns.Count > 0)
            {
                Query += " GROUP BY ";
                foreach (string Column in _groupByColumns)
                {
                    Query += Column + ',';
                }
                Query = Query.TrimEnd(',');
                Query += ' ';
            }

            // Output having statement
            if (Having.ClauseLevels > 0)
            {
                // Check if a Group By Clause was set
                if (_groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }
                if (buildCommand)
                    Query += " HAVING " + Having.BuildWhereStatement(true, ref command);
                else
                    Query += " HAVING " + Having.BuildWhereStatement();
            }

            // Output OrderBy statement
            if (_orderByStatement.Count > 0)
            {
                Query += " ORDER BY ";
                foreach (OrderByClause Clause in _orderByStatement)
                {
                    string OrderByClause = "";
                    switch (Clause.SortOrder)
                    {
                        case Sort.Ascending:
                            OrderByClause = Clause.FieldName + " ASC"; break;
                        case Sort.Descending:
                            OrderByClause = Clause.FieldName + " DESC"; break;
                    }
                    Query += OrderByClause + ',';
                }
                Query = Query.TrimEnd(','); // Trim de last AND inserted by foreach loop
                Query += ' ';
            }

            if (buildCommand)
            {
                // Return the build command
                command.CommandText = Query;
                return command;
            }
            else
            {
                // Return the built query
                return Query;
            }
        }        
    }
}
