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
    [Route("api/v1/dsaba")]
    [ApiController]
    public class DsaBaController : BaseController
    {
        ICustomerBLL customerBLL;
        IConfiguration configuration;
        public DsaBaController(ICustomerBLL _customerBLL, IConfiguration iConfig)
        {
            customerBLL = _customerBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get DsaBa Details</summary>
        /// <param name="DsaBaRequest"><see cref="DsaBaRequest"/></param>
        /// <returns><see cref="DsaBaResponse"/></returns>

        [HttpGet]
        public ActionResult<DsaBaResponse> Get([FromQuery]DsaBaRequest dsaBaRequest)
        {
            if (ModelState.IsValid)
            {
                //CustomerBLL customerBLL = new CustomerBLL();
                DsaBaResponse branchResponse = new DsaBaResponse();
                branchResponse = customerBLL.getDsaBa(dsaBaRequest);
                return Ok(branchResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}