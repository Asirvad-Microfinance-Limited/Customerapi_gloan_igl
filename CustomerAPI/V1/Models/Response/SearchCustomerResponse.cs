using APIBaseClassLibrary.V1.Models.Response;
using CustomerAPI.V1.Models.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Response
{
    public class SearchCustomerResponse:BaseResponse
    {
        public List<SearchCustomerProperties> searchCustomerList { get; set; }
    }
}
