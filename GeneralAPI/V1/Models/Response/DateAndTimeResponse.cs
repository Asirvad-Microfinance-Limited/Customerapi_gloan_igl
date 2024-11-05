using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APIBaseClassLibrary.V1.Models.Response;
namespace GeneralAPI.V1.Models.Response
{
    public class DateAndTimeResponse: BaseResponse
    {
        [JsonProperty("systemDate")]
        public string SYSDATE { get; set; }
    }
}
