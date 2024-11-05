using APIBaseClassLibrary.V1.Models.Response;
using  GeneralAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Response
{
    public class PreNameResponse : BaseResponse
    {
        public List<PreNameProperties> preNameList { get; set; }
    }
}
