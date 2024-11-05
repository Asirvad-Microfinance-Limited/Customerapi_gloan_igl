using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.V1.Models.Properties
{
    public class BankFillProperties
    {
        public string IFSCCODE { get; set; }
        public long BANKID { get; set; }
        public string BANKNAME { get; set; }

    }
}
