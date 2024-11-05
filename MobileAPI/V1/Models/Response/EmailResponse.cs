using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileAPI.V1.Models.Response
{
    public class EmailResponse : BaseResponse
    {
       public string message { get; set; }
    }

    public class SaveEmailOTPResponse : BaseResponse

    {
        public string message { get; set; }
    }

    public class SendEmailOTPResponse : BaseResponse
    {
        public string otp { get; set; }
    }
}

