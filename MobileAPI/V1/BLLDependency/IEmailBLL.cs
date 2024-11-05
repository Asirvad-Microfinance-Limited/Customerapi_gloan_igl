using MobileAPI.V1.Models.Request;
using MobileAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileAPI.V1.BLLDependency
{
    public interface IEmailBLL

    {
        SendEmailOTPResponse sendEmailOtp(SendEmailOTPRequest sendOTPRequest);
        SaveEmailOTPResponse saveEmailOTP(SaveEmailOTPRequest saveEmailOTPRequest);


    }
}
