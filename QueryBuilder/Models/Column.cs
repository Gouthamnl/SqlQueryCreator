using Newtonsoft.Json;
using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace QueryBuilder.Models
{
    public class Column
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Comparison Operator { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FieldName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FieldValue { get; set; }
        
    }
}
