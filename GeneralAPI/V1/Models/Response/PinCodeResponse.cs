using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APIBaseClassLibrary.V1.Models.Response;
using GeneralAPI.V1.Models.Properties;

namespace GeneralAPI.V1.Models.Response
{
    public class PinCodeResponse : BaseResponse
    {
        
        public long pinCode { get; set; }

        public List<DetailsProperties> detailsList { get; set; }
    }
}
