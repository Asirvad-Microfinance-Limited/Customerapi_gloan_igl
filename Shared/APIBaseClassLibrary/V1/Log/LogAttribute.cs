using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using APIBaseClassLibrary.V1.Models.Request;
using DataAccessLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static GlobalLibrary.GlobalVariables;

namespace APIBaseClassLibrary.V1.Log
{
    public class LogAttribute :ActionFilterAttribute, IActionFilter, IResultFilter
    {
        string _branchId = string.Empty;
        string _userId = string.Empty;
        public async override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            try
            {
                AppConfiguration appConfiguration = new AppConfiguration();

                if (appConfiguration._isLog == "1")
                {
                    StringValues authorizationToken;
                    LogBase _logBase = new LogBase();
                    actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
                    if (!string.IsNullOrWhiteSpace(authorizationToken.ToString()))
                    {
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                        var SecurityToken = handler.ReadToken(authorizationToken) as JwtSecurityToken;

                        var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                        string[] value = jti.Split(',');
                        _branchId = value[0];
                        _userId = value[1];
                    }
                    else
                    {
                        _branchId = "0";
                        _userId = "000000";
                    }

                    string postData = string.Empty;

                    string Scheme = actionContext.HttpContext.Request.Scheme + "://";
                    string PathBase = actionContext.HttpContext.Request.PathBase;
                    string Path = actionContext.HttpContext.Request.Path;
                    string Host = actionContext.HttpContext.Request.Host.ToString();
                    string QueryString = actionContext.HttpContext.Request.QueryString != null ? actionContext.HttpContext.Request.QueryString.ToString() : null;
                    var url = Scheme + Host + PathBase + Path + QueryString;

                    var Method = actionContext.HttpContext.Request.Method.ToString();
                    string actionName = actionContext.RouteData.Values["action"].ToString();
                    string controllerName = actionContext.RouteData.Values["controller"].ToString();
                    string LogId = DateTime.Now.ToString("MMddyyyyhhmmss");

                    actionContext.HttpContext.Items["url"] = url;
                    actionContext.HttpContext.Items["LogId"] = LogId;
                    actionContext.HttpContext.Items["Method"] = Method;
                    actionContext.HttpContext.Items["actionName"] = actionName;
                    actionContext.HttpContext.Items["controllerName"] = controllerName;
                    actionContext.HttpContext.Items["_userId"] = _userId;
                    actionContext.HttpContext.Items["_branchId"] = _branchId;

                    postData = JsonConvert.SerializeObject(actionContext.ActionArguments);

                    var object1 = JObject.Parse(postData);
                    object1.Descendants().OfType<JProperty>().Where(attr => attr.Name.ToLower().StartsWith("password")).ToList().ForEach(attr => attr.Remove()); // removing unwanted attributes
                    postData = object1.ToString();



                    GlAuditLogRequest glAuditLogRequest = new GlAuditLogRequest();
                    glAuditLogRequest.LOG_ID = LogId;
                    glAuditLogRequest.BRANCH_ID = _branchId;
                    glAuditLogRequest.EMP_CODE = _userId;
                    glAuditLogRequest.URL = url;
                    glAuditLogRequest.CONTROLLER_NAME = controllerName;
                    glAuditLogRequest.ACTION_NAME = actionName;
                    glAuditLogRequest.METHOD = Method;
                    glAuditLogRequest.REQUEST = postData;
                    glAuditLogRequest.REQUEST_TIME = DateTime.Now.ToLongTimeString();
                    await _logBase.addAuditLog(glAuditLogRequest);
                }


            }
            catch (Exception ex)
            {

            }
        }

        public string generateOTP()
        {
            int lenthofpass = 6;
            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0,";
            char[] sep = new[] { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i <= lenthofpass - 1; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }

        public async override void OnResultExecuted(ResultExecutedContext actionContext)
        {
            try
            {
                AppConfiguration appConfiguration = new AppConfiguration();

                if (appConfiguration._isLog == "1")
                {
                    StringValues authorizationToken;
                    LogBase _logBase = new LogBase();
                    actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
                    if (!string.IsNullOrWhiteSpace(authorizationToken.ToString()))
                    {
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                        var SecurityToken = handler.ReadToken(authorizationToken) as JwtSecurityToken;

                        var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                        string[] value = jti.Split(',');
                        _branchId = value[0];
                        _userId = value[1];
                    }
                    else
                    {
                        _branchId = "0";
                        _userId = "000000";
                    }

                    string postData = string.Empty;

                    object logId = actionContext.HttpContext.Items["LogId"];
                    var response = JsonConvert.SerializeObject(actionContext.Result);
                    string statusResponse = string.Empty;
                    if (actionContext.Result is OkObjectResult)
                    {
                        OkObjectResult jsonResult = actionContext.Result as OkObjectResult;

                        foreach (var prop in jsonResult.Value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            string propName = prop.Name;
                            var propValue = prop.GetValue(jsonResult.Value, null);
                            if (propName == "status")
                            {
                                ResponseStatus responseStatus = (ResponseStatus)propValue;
                                statusResponse = responseStatus.code.ToString();


                            }
                        }
                    }

                    //await log.writeLog("Response:" + response);
                    GlAuditLogUpdateRequest glAuditLogUpdateRequest = new GlAuditLogUpdateRequest();
                    glAuditLogUpdateRequest.LOG_ID = logId.ToString();
                    glAuditLogUpdateRequest.RESPONSE = response;
                    glAuditLogUpdateRequest.RESPONSE_TIME = DateTime.Now.ToLongTimeString();
                    glAuditLogUpdateRequest.RESPONSE_STATUS = statusResponse;
                    await _logBase.updateAuditLog(glAuditLogUpdateRequest);
                }
            }
            catch (Exception ex)
            {

            }

        }

    }
}
