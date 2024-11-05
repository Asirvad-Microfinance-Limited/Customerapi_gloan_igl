using APIBaseClassLibrary.V1.Controllers;
using Employee.V1.BLL;
using EmployeeAPI.Models;
using EmployeeAPI.Models.Request;
using EmployeeAPI.V1.BLL;
using EmployeeAPI.V1.Models.Request;
using EmployeeAPI.V1.Models.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace EmployeeAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
  
    public class LoginController : BaseController
    {
        ILoginBLL login;
        IConfiguration configuration;
        public LoginController(ILoginBLL _login, IConfiguration iConfig)
        {
            login = _login;
            configuration = iConfig;
        }


        // GET api/login
        [HttpPost]
        public ActionResult<LoginResponse> Post([FromBody] LoginRequest userLogin)
        {
            if (ModelState.IsValid)
            {
                //LoginBLL login = new LoginBLL();
                return Ok(login.userLogin(userLogin));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        // DELETE api/login/5
        [HttpDelete]
        public ActionResult<LogoutResponse> Delete(LogoutRequest logoutRequest)
        {
            LogoutResponse logoutResponse = new LogoutResponse();
            if (ModelState.IsValid)
            {
                //LoginBLL login = new LoginBLL();
                return Ok(login.userLogout(logoutRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}