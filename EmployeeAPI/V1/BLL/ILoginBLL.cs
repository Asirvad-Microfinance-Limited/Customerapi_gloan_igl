using EmployeeAPI.Models;
using EmployeeAPI.Models.Request;
using EmployeeAPI.V1.Models.Request;
using EmployeeAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.V1.BLL
{
    public interface ILoginBLL
    {
        LoginResponse userLogin(LoginRequest request);
        LoginResponse verifyOTP(VerifyLoginOTPRequest request);
        ResendLoginOTPResponse reSendOTP(ResendLoginOTPRequest request);
        LogoutResponse userLogout(LogoutRequest logoutRequest);
    }
}
