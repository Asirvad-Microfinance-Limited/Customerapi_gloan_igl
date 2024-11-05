using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using DataAccessLibrary;
using KYCAPI.V1.BLL;
using KYCAPI.V1.BLLDependency;
using KYCAPI.V1.Models.Request;
using KYCAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KYCAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EKYCController : BaseController
    {
        IConfiguration configuration;
        IEKYCBLL eKYCBLL;
        public EKYCController(IConfiguration iConfig, IEKYCBLL _eKYCBLL)
        {
            configuration = iConfig;
            eKYCBLL = _eKYCBLL;
        }

        #region EKYCGet
        [HttpGet]
        public ActionResult<EKYCResponse> Get([FromQuery]EKYCGetRequest eKYCGetRequest)
        {
           
            if (ModelState.IsValid)
            {
            //    EKYCBLL eKYCBLL = new EKYCBLL(configuration);
                return Ok(eKYCBLL.EKYCGet(eKYCGetRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion EKYCGet

        #region EKYCPut
        [HttpPut]
        public ActionResult<EkycPostResponse> Put([FromBody]EKYCRequest eKYCRequest)
        {
            if (ModelState.IsValid)
            {
                //EKYCBLL eKYCBLL = new EKYCBLL(configuration);
                return Ok(eKYCBLL.updateEKYC(eKYCRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion EKYCPut
    }
}