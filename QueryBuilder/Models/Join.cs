using Newtonsoft.Json;
using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Models
{
    public class Join
    {
        [JsonProperty("clause")]
        public ClauseType Clause { get; set; }
        [JsonProperty("fromTable")]
        public string FromTable { get; set; }
        [JsonProperty("fromColumn")]
        public string FromColumn { get; set; }
        [JsonProperty("toTable")]
        public string ToTableName { get; set; }
        [JsonProperty("toColumn")]
        public string ToColumn { get; set; }
        [JsonProperty("operator")]
        public Comparison Operator { get; set; }
    }
}
