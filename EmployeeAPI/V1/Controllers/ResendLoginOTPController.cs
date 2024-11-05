using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using Employee.V1.BLL;
using EmployeeAPI.Models;
using EmployeeAPI.Models.Request;
using EmployeeAPI.V1.BLL;
using EmployeeAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeeAPI.V1.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ResendLoginOTPController : BaseController
    {
        ILoginBLL login;
        IConfiguration configuration;
        public ResendLoginOTPController(ILoginBLL _login, IConfiguration iConfig)
        {
            login = _login;
            configuration = iConfig;
        }
        [HttpPost]
        public ActionResult<ResendLoginOTPResponse> Post([FromQuery] ResendLoginOTPRequest request)
        {
            if (ModelState.IsValid)
            {
                //LoginBLL login = new LoginBLL();
                return Ok(login.reSendOTP(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}