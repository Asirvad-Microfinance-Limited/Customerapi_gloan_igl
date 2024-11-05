using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.V1.Models.Properties
{
    public class AccountTypeProperties
    {
        [JsonProperty("accountType")]
        public long ACC_TYPE { get; set; }

        [JsonProperty("accountName")]
        public string ACCOUNT_NAME { get; set; }
    }
}
