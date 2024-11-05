using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using GeneralAPI.V1.BLL;
using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeneralAPI.V1.Controllers
{
    [Route("api/v1/pincode")]
    [ApiController]
    public class PinCodeController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public PinCodeController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get PinCode</summary>
        /// <param name="PinCodeRequest"><see cref="PinCodeRequest"/></param>
        /// <returns><see cref="PinCodeResponse"/></returns>
        [HttpGet]
        public ActionResult<PinCodeResponse> Get([FromQuery]PinCodeRequest pinCodeRequest)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                PinCodeResponse pinCodeResponse = new PinCodeResponse();
                pinCodeResponse = generalBLL.getPinCode(pinCodeRequest);
                return Ok(pinCodeResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}