using QueryBuilder.Services;
using System;
using QueryBuilder.Models;
using System.IO;
using Newtonsoft.Json;
using QueryBuilder.Logger;
using QueryBuilder.Enums;
using System.Collections.Generic;

namespace SqlQueryCreator.Operations
{
    /// <summary>
    /// Generate queries based on input data
    /// </summary>
    public class QueryGenerator
    {
        private readonly string _basePath; 
        private const string DataLocation = "TestData";

        public QueryGenerator()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), DataLocation);
        }

        /// <summary>
        /// Build SQL query with where conditions only
        /// </summary>
        /// <param name="query"></param>
        public void WhereQuery(QueryBuilderService query)
        {
            try
            {
                string path;
                path = Path.Combine(_basePath, "WhereData.json");
                string jsonData = FileReader(path);
                Data whereData = JsonConvert.DeserializeObject<Data>(jsonData);

                if (whereData != null)
                {
                    query.SelectFromTable(whereData.BaseData.TableName);

                    if (whereData.BaseData.SelectedColumns != null)
                    {
                        query.SelectColumns(whereData.BaseData.SelectedColumns);
                    }

                    if (whereData.BaseData.Columns != null)
                    {
                        CreateWhere(query, whereData.BaseData.Columns);
                    }                    
                    Console.WriteLine(query.BuildQuery());
                }                
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message, LogLevel.Error);
            }
        }

        /// <summary>
        /// Build SQL query with multiple joins and where conditions
        /// </summary>
        /// <param name="query"></param>
        public void JoinQuery(QueryBuilderService query)
        {
            try
            {
                string path;
                path = Path.Combine(_basePath, "JoinData.json");
                string jsonData = FileReader(path);
                if (!string.IsNullOrEmpty(jsonData))
                {

                    JoinData joinData = JsonConvert.DeserializeObject<JoinData>(jsonData);
                    query.SelectFromTable(joinData.TableName);
                    if (joinData.SelectedColumns != null)
                    {
                        query.SelectColumns(joinData.SelectedColumns);
                    }

                    if (joinData.Columns != null)
                    {
                        CreateWhere(query, joinData.Columns);
                    }
                    if(joinData.Join != null)
                    {
                        CreateJoin(query, joinData.Join);
                    }                                       
                    Console.WriteLine(query.BuildQuery());
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message, LogLevel.Error);
            }
        }

        /// <summary>
        /// Read json data from specific location
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string FileReader(string path)
        {
            try
            {
                string jsonData = File.ReadAllText(path);
                return jsonData;
            }
            catch(FileNotFoundException fn)
            {
                Logger.Log($"File Not Found. File name : {fn.FileName}", LogLevel.Error);
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message, LogLevel.Error);                
            }
            return string.Empty;
        }

        /// <summary>
        /// Build where conditions based on json data file
        /// </summary>
        /// <param name="query"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static QueryBuilderService CreateWhere(QueryBuilderService query,List<Column> columns)
        {
            try
            {
                foreach (var col in columns)
                {
                    query.AddWhere(col.FieldName, col.Operator, col.FieldValue);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogLevel.Error);
            }
            return query;
        }

        /// <summary>
        /// Build join conditions based on json data file
        /// </summary>
        /// <param name="query"></param>
        /// <param name="joins"></param>
        /// <returns></returns>
        private static QueryBuilderService CreateJoin(QueryBuilderService query, List<Join> joins)
        {
            try
            {
                foreach (var join in joins)
                {
                    query.AddJoin(join.Clause,
                              join.ToTableName, join.ToColumn,
                              join.Operator,
                              join.FromTable, join.FromColumn);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message, LogLevel.Error);
            }
            return query;
        }
    }
}
