using APIBaseClassLibrary.V1.Models.Response;
using BankAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.V1.Models.Response
{
    public class AccountTypeResponse : BaseResponse
    {
        public List<AccountTypeProperties> accountTypeList { get; set; }
    }
}
