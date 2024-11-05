using System;
using System.Data;
using APIBaseClassLibrary.V1.Log;
using APIBaseClassLibrary.V1.TokenAttribute;
using DataAccessLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using static GlobalValues.GlobalVariables;

namespace APIBaseClassLibrary.V1.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(TokenValidator))]
    //[ApiController]
   
  //[ServiceFilter(typeof(LogAttribute))]
    public class BaseController : Controller
    {

        public BaseController()
        {

        }
    }
}