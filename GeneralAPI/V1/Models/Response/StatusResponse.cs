using  GeneralAPI.V1.Models.Properties;
using APIBaseClassLibrary.V1.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Response
{
    public class StatusResponse : BaseResponse
    {
        public List<StatusProperties> statusList { get; set; }
    }
}
