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
    [Route("api/v1/pincodetails")]
    [ApiController]
    public class PinCodeDetailsController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public PinCodeDetailsController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }
        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get pincode details </summary>
        /// <param name="DetailsRequest"><see cref="DetailsRequest"/></param>
        /// <returns><see cref="DetailsResponse"/></returns>
        [HttpGet]
        public ActionResult<DetailsResponse> Get([FromQuery]DetailsRequest detailsRequest)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                DetailsResponse detailsResponse = new DetailsResponse();
                detailsResponse = generalBLL.getDetails(detailsRequest);
                return Ok(detailsResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}