using APIBaseClassLibrary.V1.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Response
{
    public class ConfigInsertResponse : BaseResponse
    {

    }

    public class ConfigUpdateResponse : BaseResponse
    {

    }

    public class ConfigDetailsResponse : BaseResponse
    {
        public List<ConfigDetailsProperties> configDetails { get; set; }
    }

    public class ConfigDetailsProperties
    {
        [JsonProperty("configId")]
        public long CONFIG_ID { get; set; }
        [JsonProperty("configName")]
        public string CONFIG_NAME { get; set; }
        [JsonProperty("configStatus")]
        public CheckStatus CONFIG_STATUS { get; set; }
    }
}
