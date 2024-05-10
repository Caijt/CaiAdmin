using CaiAdmin.Entity.Sys;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class ConfigDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ConfigKey Key { get; set; }
        public string Value { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ConfigType Type { get; set; }
    }




}
