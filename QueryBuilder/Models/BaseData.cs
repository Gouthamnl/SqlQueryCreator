using Newtonsoft.Json;
using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Models
{
    public class BaseData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tableName")]
        public string TableName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "clause")]
        public ClauseType Clause { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "columns")]
        public List<Column> Columns { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "selectedColumns")]
        public string[] SelectedColumns { get; set; }
    }
}
