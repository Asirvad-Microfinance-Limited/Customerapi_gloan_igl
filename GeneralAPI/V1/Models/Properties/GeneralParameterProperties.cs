using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class GeneralParameterProperties
    {
        [JsonProperty("parameterValue")]
        public string PARMTR_VALUE { get; set; }
        [JsonProperty("parameterName")]
        public string PARMTR_NAME { get; set; }
    }
}
