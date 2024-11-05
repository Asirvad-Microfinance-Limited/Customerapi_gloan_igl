using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobileAPI.V1.BLLDependency;
using MobileAPI.V1.Models.Request;
using MobileAPI.V1.Models.Response;
using MoblieAPI.V1.BLL;

namespace MobileAPI.V1.Controllers
{
    [Route("api/v1/otp")]
    [ApiController]
    public class OTPController : BaseController
    {

        IConfiguration configuration;
        IMobileBLL mobile;
        public OTPController(IConfiguration iConfig, IMobileBLL _mobile)
        {
            mobile = _mobile;
            configuration = iConfig;
        }

       // IConfiguration configuration;
      // public OTPController(IConfiguration iConfig)
      //  {
      //      configuration = iConfig;
      //  }
        [HttpGet]
        public ActionResult<SendOTPResponse> Get([FromQuery]SendOTPRequest sendOTPRequest)
        {
            if (ModelState.IsValid)
            {
             //   MobileBLL mobileBLL = new MobileBLL();
                SendOTPResponse branchResponse = new SendOTPResponse();
                branchResponse = mobile.sendOtp(sendOTPRequest);
                return Ok(branchResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public ActionResult<VerifyOTPResponse> Post([FromBody]VerifyOTPRequest verifyOTPRequest)
        {
            if (ModelState.IsValid)
            {
               // MobileBLL mobileBLL = new MobileBLL();
                VerifyOTPResponse verifyOTPResponse = new VerifyOTPResponse();
                verifyOTPResponse = mobile.verifyOTP(verifyOTPRequest);
                return Ok(verifyOTPResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}