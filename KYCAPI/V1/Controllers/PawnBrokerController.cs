using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
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
    public class PawnBrokerController : BaseController
    {
        IConfiguration configuration;
        IEKYCBLL eKYCBLL;
        public PawnBrokerController(IConfiguration iConfig, IEKYCBLL _eKYCBLL)
        {
            configuration = iConfig;
            eKYCBLL = _eKYCBLL;
        }

        #region PawnBrokerGet

        [HttpGet]
        public ActionResult<PawnBrokerResponse> Get([FromQuery]PawnBrokerRequest pawnBrokerRequest)
        {
            if (ModelState.IsValid)
            {
               // EKYCBLL eKYCBLL = new EKYCBLL(configuration);
                return Ok(eKYCBLL.PawnBrokerGetImage(pawnBrokerRequest));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion PawnBrokerGet
    }
}