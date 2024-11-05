using MobileAPI.V1.Models.Request;
using MobileAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileAPI.V1.BLLDependency
{
    public interface IMobileBLL
    {
        MobileResponse getbaMobile(MobileRequest request);
        SendOTPResponse sendOtp(SendOTPRequest sendOTPRequest);
        VerifyOTPResponse verifyOTP(VerifyOTPRequest verifyOTPRequest);

    }
}
