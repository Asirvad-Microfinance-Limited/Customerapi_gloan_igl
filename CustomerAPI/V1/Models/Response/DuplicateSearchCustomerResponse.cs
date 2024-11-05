using APIBaseClassLibrary.V1.Models.Response;
using CustomerAPI.V1.Models.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.V1.Models.Response
{
    public class DuplicateSearchCustomerResponse:BaseResponse
    {
        [JsonProperty("isDuplicate")]
        public bool isDuplicate { get; set; }
    }

    public class matchingCustomerResponse : BaseResponse
    {
        public List<matchingCustomerProperties> matchingCustomers { get; set; }
    }

    public class matchingCustomerProperties
    {
        public string group { get; set; }
        public string custId { get; set; }
        public string name { get; set; }
        public string phone2 { get; set; }
        public string fatHus { get; set; }
        public string address { get; set; }
        public string dateOfBirth { get; set; }
        public string idNumber { get; set; }
       
    }
    

}
