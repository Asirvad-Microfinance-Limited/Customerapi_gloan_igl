using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using CustomerAPI.V1.BLL;
using CustomerAPI.V1.Models.Request;
using CustomerAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CustomerAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OGLCustomerController : BaseController
    {

        IOGLCustomer customerBLL;
        IConfiguration configuration;
        public OGLCustomerController(IOGLCustomer _customerBLL, IConfiguration iConfig)
        {
            customerBLL = _customerBLL;
            configuration = iConfig;
        }


        // GET: api/OGLCustomer
        [HttpGet]
        public ActionResult<OGLCustomerResponse> Get([FromQuery]OGLCustomerRequest customerDataRequest)
        {
            if (ModelState.IsValid)
            {
               // OGLCustomer customerBLL = new OGLCustomer();
                OGLCustomerResponse customerDataResponse = new OGLCustomerResponse();
                customerDataResponse = customerBLL.searchOGLCustomer(customerDataRequest);
                return Ok(customerDataResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
