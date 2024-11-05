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
    [Route("api/v1/email")]
   // [ApiController]
    public class EmailController : BaseController
    {
        IConfiguration configuration;
        IEmailBLL email;
        public EmailController(IConfiguration iConfig, IEmailBLL _email)
        {
            email = _email;
            configuration = iConfig;
        }

        [HttpGet]
        public ActionResult<SendEmailOTPResponse> Get([FromQuery]SendEmailOTPRequest sendOTPRequest)
        {
            if (ModelState.IsValid)
            {
               
                SendEmailOTPResponse emailOtpResponse = new SendEmailOTPResponse();
                emailOtpResponse = email.sendEmailOtp(sendOTPRequest);
                return Ok(emailOtpResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public ActionResult<SaveEmailOTPResponse> Post([FromBody]SaveEmailOTPRequest saveEmailOTPRequest)
        {
            if (ModelState.IsValid)
            {
                // MobileBLL mobileBLL = new MobileBLL();
                SaveEmailOTPResponse saveEmailOTPResponse = new SaveEmailOTPResponse();
                saveEmailOTPResponse = email.saveEmailOTP(saveEmailOTPRequest);
                return Ok(saveEmailOTPResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}