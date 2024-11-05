using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBaseClassLibrary.V1.Controllers;
using GeneralAPI.V1.BLL;
using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeneralAPI.V1.Controllers
{
    [Route("api/v1/OccupationSubCategory")]
    [ApiController]
    public class OccupationController : Controller
    {
        IGeneralBLL generalBLL;
        IConfiguration configuration;
        public OccupationController(IGeneralBLL _generalBLL, IConfiguration iConfig)
        {
            generalBLL = _generalBLL;
            configuration = iConfig;
        }

        [HttpGet("OccupationSubCategory")]

        public ActionResult<OccupationSubCategoryResponse> Get([FromQuery]OccupationSubCategoryRequest  Request)
        {
            if (ModelState.IsValid)
            {
                //GeneralBLL generalBLL = new GeneralBLL();
                OccupationSubCategoryResponse  Response = new OccupationSubCategoryResponse();
                Response = generalBLL.getOccupationSubCategory(Request);
                return Ok(Response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}