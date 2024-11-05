using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using GeneralAPI.V1.BLL;
using GeneralAPI.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeneralAPI.V1.Controllers
{
    [Route("api/v1/prenames")]
    [ApiController]
    public class PreNamesController : BaseController
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public PreNamesController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }

        /// <Created>Aravind R - 100231</Created>
        /// <summary>Get PreNameList</summary>
        /// <returns><see cref="PreNameResponse"/></returns>
        [HttpGet]
        public ActionResult<PreNameResponse> Get()
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                PreNameResponse preNameResponse = new PreNameResponse();
                preNameResponse = generalBLL.getPreNameList();
                return Ok(preNameResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}