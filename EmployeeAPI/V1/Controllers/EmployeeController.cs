using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using EmployeeAPI.V1.BLL;
using EmployeeAPI.V1.Models.Request;
using EmployeeAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeeAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        IEmployeeBLL employeeBLL;
        IConfiguration configuration;
        public EmployeeController(IEmployeeBLL _employeeBLL, IConfiguration iConfig)
        {
            employeeBLL = _employeeBLL;
            configuration = iConfig;
        }
        // GET: api/Employee
        [HttpGet]
        public ActionResult<EmployeeResponse> Get([FromQuery]EmployeeRequest employee)
        {
            if (ModelState.IsValid)
            {
               // EmployeeBLL employeeBLL = new EmployeeBLL();
                return Ok(employeeBLL.getEmployeeDetails(employee));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("branch")]
        public ActionResult<EmployeeBranchResponse> Get([FromQuery]EmployeeBranchRequest employee)
        {
            if (ModelState.IsValid)
            {
                //EmployeeBLL employeeBLL = new EmployeeBLL();
                return Ok(employeeBLL.getEmployeeBranch(employee));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        



        //// GET: api/Employee?employeeID=1&mobileNumber=8137035564
        //[HttpGet]
        //public string Get([FromQuery]int employeeID, string mobileNumber)
        //{
        //    return "value";
        //}
    }
}
