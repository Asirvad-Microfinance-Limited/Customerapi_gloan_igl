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
    [Route("api/v1/generalparameter")]
    [ApiController]
    public class GeneralParameterController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public GeneralParameterController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get General Parameter details</summary>
        /// <param name="GeneralParameterRequest"><see cref="GeneralParameterRequest"/></param>
        /// <returns><see cref="GeneralParameterResponse"/></returns>
        [HttpGet]
        public ActionResult<GeneralParameterResponse> Get([FromQuery]GeneralParameterRequest generalParameterRequest)
        {
            if (ModelState.IsValid)
            {
               // GeneralBLL generalBLL = new GeneralBLL();
                GeneralParameterResponse generalParameterResponse = new GeneralParameterResponse();
                generalParameterResponse = generalBLL.getGeneralParameter(generalParameterRequest);
                return Ok(generalParameterResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}