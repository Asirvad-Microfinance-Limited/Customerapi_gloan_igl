using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1.TokenAttribute
{


    public class TokenValidator : ActionFilterAttribute
    {
        IToken token;
        public TokenValidator(IToken _token)
        {
            token = _token;
        }


        private bool tokenValidationRequired = true;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool validUser = false;
            if (isServiceAvailable())   /////////// return true if the api service is ready to do
            {
                if (isDBServerAvailable())   /////////// return true if the DB server is available
                {
                    StringValues authorizationToken;
                    filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
                    string controllerName = filterContext.RouteData.Values["controller"].ToString();
                    string actionName = filterContext.RouteData.Values["action"].ToString();

                    string[] cname = { "employee", "login" };


                    //if ((controllerName.ToLower() != "login" && actionName.ToLower() != "post") && (controllerName.ToLower() != "employee" && actionName.ToLower() != "get"))
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

                    if ((controllerName.ToLower() == "login" && actionName.ToLower() == "post") || (controllerName.ToLower() == "employee" && actionName.ToLower() == "get") || (controllerName.ToLower() == "log" && actionName.ToLower() == "post"))
                    {
                       

                        if (isAutherized(authorizationToken))   /////////// return true if the user is autherized is ready to do
                        {
                            validUser = true;
                        }
                    }
                    else
                    {
                        if (isTokenAvailable(authorizationToken))   /////////// return true if the token is available in the token
                        {
                            if (isValidToken(authorizationToken))   /////////// return true if the token is valid
                            {
                                if (isAutherized(authorizationToken))   /////////// return true if the user is autherized is ready to do
                                {
                                    validUser = true;
                                }
                            }
                        }
                    }





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
                   
                   isValidToken = token.isValidToken(userContextObj);
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
    }
}
