using APIBaseClassLibrary.V1.Models.Response;
using CustomerAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Response
{
    public class StatusResponse : BaseResponse
    {
        public List<GeneralStatusProperties> statusList { get; set; }
    }
}
