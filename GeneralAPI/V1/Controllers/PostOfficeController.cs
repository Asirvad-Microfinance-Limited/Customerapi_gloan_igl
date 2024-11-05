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
    [Route("api/v1/postoffice")]
    [ApiController]
    public class PostOfficeController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public PostOfficeController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get PostOffice details</summary>
        /// <param name="PostOfficeRequest"><see cref="PostOfficeRequest"/></param>
        /// <returns><see cref="PostOfficeResponse"/></returns>
        [HttpGet]
        public ActionResult<PostOfficeResponse> Get([FromQuery]PostOfficeRequest postOfficeRequest)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                PostOfficeResponse postOfficeResponse = new PostOfficeResponse();
                postOfficeResponse = generalBLL.getPostOffice(postOfficeRequest);
                return Ok(postOfficeResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}