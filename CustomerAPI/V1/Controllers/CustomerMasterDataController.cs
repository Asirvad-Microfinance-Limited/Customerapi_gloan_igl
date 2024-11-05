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
    [Route("api/v1/customermasterdata")]
    [ApiController]
    public class CustomerMasterDataController : BaseController
    {
        ICustomerBLL customerBLL;
        IConfiguration configuration;
        public CustomerMasterDataController(ICustomerBLL _customerBLL, IConfiguration iConfig)
        {
            customerBLL = _customerBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get Customer Master Data</summary>
        /// <returns><see cref="CustomerMasterDataResponse"/></returns>
        [HttpGet]
        public ActionResult<CustomerMasterDataResponse> Get()
        {
            if (ModelState.IsValid)
            {
                //CustomerBLL customerBLL = new CustomerBLL();
                CustomerMasterDataResponse customerMasterDataResponse = new CustomerMasterDataResponse();
                customerMasterDataResponse = customerBLL.getMasterDataForCustomer();
                return Ok(customerMasterDataResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Post Modify PEP details in a customer ID /// 3-oct-2020
        [HttpPost("UpdatePEPDetails")]
        public ActionResult<UpdatePEPDetailsResponse> UpdatePEPDetails([FromBody] UpdatePEPDetailsRequest request)
        {
            if (ModelState.IsValid)
            {
                return Ok(customerBLL.UpdatePEPDetails(request));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}