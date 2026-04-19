using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using APIBaseClassLibrary.V1.Log;
using DataAccessLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using static GlobalLibrary.GlobalVariables;

namespace APIBaseClassLibrary.V1.Controllers
{
   
    [ValidateToken]
    [LogAttribute]
    public class BaseController : Controller
    {

        public BaseController()
        {
          
           
            
        }

        public class ValidateToken : ActionFilterAttribute
        {

            private bool tokenValidationRequired = true;
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                bool validUser = false;
                if (isServiceAvailable())   /////////// return true if the api service is ready to do
                {
                    if (isDBServerAvailable())   /////////// return true if the DB server is available
                    {
                        //validUser = true;
                        //string requestOrigin1 = filterContext.HttpContext.Request.Headers["Origin"];
                        //string requestOrigin = filterContext.HttpContext.Request.Host.Value.ToString();
                        //if (requestOrigin == "amfluat.macom.in")
                        //{
                        //    validUser = true;
                        //    return;
                        //}
                        //validUser = true;
                        StringValues authorizationToken;
                        filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);

                        filterContext.HttpContext.Items["authorizationToken"] = authorizationToken;
                        string controllerName = filterContext.RouteData.Values["controller"].ToString();
                        string actionName = filterContext.RouteData.Values["action"].ToString();

                        //if (controllerName.ToLower() != "login" && actionName.ToLower() != "post")
                        //{
                        //    if (isTokenAvailable(authorizationToken))   /////////// return true if the token is available in the token
                        //    {
                        //        if (isValidToken(authorizationToken))   /////////// return true if the token is valid
                        //        {
                        //            if (isAutherized(authorizationToken))   /////////// return true if the user is autherized is ready to do
                        //            {
                        //                validUser = true;
                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (isAutherized(authorizationToken))   /////////// return true if the user is autherized is ready to do
                        //    {
                        //        validUser = true;
                        //    }
                        //}
                        validUser = true;
                    }
                }

                if (!validUser)
                {
                    filterContext.Result = new StatusCodeResult(403);
                    return;
                }
            }

            private bool isTokenAvailable(string userContextObj)
            {
                bool checkToken = false;
                if (userContextObj != null)
                {
                    checkToken = true;
                }
                return checkToken;
            }

            private bool isAutherized(string userContextObj)
            {
                return true;
            }

            private bool isValidToken(string userContextObj)
            {
                bool isValidToken = false;

                try
                {
                    OracleParameter[] parm_coll = new OracleParameter[3];
                    DBAccessHelper helper = new DBAccessHelper();
                    parm_coll[0] = new OracleParameter("var_token", OracleDbType.Varchar2);
                    parm_coll[0].Value = userContextObj;
                    parm_coll[0].Direction = ParameterDirection.Input;
                    parm_coll[1] = new OracleParameter("var_otp_duration", OracleDbType.Decimal);
                    parm_coll[1].Value = (int)Durations.LoginOTP;
                    parm_coll[1].Direction = ParameterDirection.Input;
                    parm_coll[2] = new OracleParameter("out_status", OracleDbType.Decimal);
                    parm_coll[2].Direction = ParameterDirection.Output;

                    helper.ExecuteNonQuery("proc_ValidateToken", parm_coll);
                    if (parm_coll[2].Value.ToString() == "1")
                    {
                        isValidToken = true;
                    }
                }
                catch (Exception ex)
                {
                    isValidToken = false;
                }
                return isValidToken;
            }

            private bool isDBServerAvailable()
            {
                return true;
            }

            private bool isServiceAvailable()
            {
                return true;
            }

            public bool tokenBranchValidation(string authorizationToken, int branchID)
            {
                bool returnvalue = false;
                try
                {
                    if (!string.IsNullOrWhiteSpace(authorizationToken.ToString()))
                    {
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                        var SecurityToken = handler.ReadToken(authorizationToken) as JwtSecurityToken;

                        var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                        string[] value = jti.Split(',');
                        if (branchID == Convert.ToInt32(value[0]))
                        {
                            returnvalue = true;
                        }
                        else
                        { returnvalue = false; }

                    }
                    else
                    {
                        returnvalue = false;
                    }
                }
                catch (Exception ex)

                { returnvalue = false; }

                return returnvalue;


            }
        }

        

    }

}
