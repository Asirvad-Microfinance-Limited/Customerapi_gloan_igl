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
    [Route("api/v1/searchcustomer")]
    [ApiController]
    public class SearchCustomerController : BaseController
    {
        ICustomerBLL customerBLL;
        IConfiguration configuration;
        public SearchCustomerController(ICustomerBLL _customerBLL, IConfiguration iConfig)
        {
            customerBLL = _customerBLL;
            configuration = iConfig;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get Customer Search Data</summary>
        /// <param name="SearchCustomerRequest"><see cref="SearchCustomerRequest"/></param>
        /// <returns><see cref="SearchCustomerResponse"/></returns>

        [HttpGet]
        public ActionResult<SearchCustomerResponse> Get([FromQuery]SearchCustomerRequest searchCustomerRequest)
        {
            if (ModelState.IsValid)
            {
               // CustomerBLL customerBLL = new CustomerBLL();
                SearchCustomerResponse searchCustomerResponse = new SearchCustomerResponse();
                searchCustomerResponse = customerBLL.searchCustomer(searchCustomerRequest);
                return Ok(searchCustomerResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}