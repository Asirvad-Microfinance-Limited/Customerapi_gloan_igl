using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using APIBaseClassLibrary.V1.Models.Request;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace APIBaseClassLibrary.V1.Log
{
    public class LogAttribute : ActionFilterAttribute, IActionFilter, IResultFilter
    {
        ILogBase _logBase;
        IConfiguration configuration;
        string isLog = "0";
        public LogAttribute(ILogBase logBase, IConfiguration configuration) 
        {
            this.configuration = configuration;
            _logBase = logBase;
            isLog = configuration.GetSection("isLog").GetSection("status").Value;
        }

        string _branchId = string.Empty;
        string _userId = string.Empty;
        public async override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            try
            {

                if (isLog == "1")
                {
                    StringValues authorizationToken;
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

                    postData = JsonConvert.SerializeObject(actionContext.ActionArguments);
                    userLogin userLogin = new userLogin();

                    var object1 = JObject.Parse(postData);
                    object1.Descendants().OfType<JProperty>().Where(attr => attr.Name.ToLower().StartsWith("password")).ToList().ForEach(attr => attr.Remove()); // removing unwanted attributes
                    postData = object1.ToString();
                    if ((controllerName.ToLower() == "login" && actionName.ToLower() == "post"))
                    {
                        JObject jObject = JObject.Parse(postData);
                        JToken jUser = jObject["userLogin"];
                        if (_branchId == "0" && _userId == "000000")
                        {
                            _userId = (string)jUser["employeeID"];
                            _branchId = (string)jUser["branchID"];
                        }
                    }
                    else if (Path == "/api/Employee" && actionName.ToLower() == "get")
                    {
                        if (_branchId == "0" && _userId == "000000")
                        {
                            JObject jObject = JObject.Parse(postData);
                            JToken jUser = jObject["employee"];
                            _userId = (string)jUser["employeeID"];
                        }

                    }

                    actionContext.HttpContext.Items["_userId"] = _userId;
                    actionContext.HttpContext.Items["_branchId"] = _branchId;

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
                if (isLog == "1")
                {
                    StringValues authorizationToken;
                    actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);

                    //  var stream = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMCwwLDAiLCJuYmYiOjE1NDEwNjYzMjMsImV4cCI6MTU0MTA2NjYyMywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.YsS97WkViUD9KufTguTe4QqoeI6fRDb-CITgzrOQng4";

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
                    // Log log = new Log();

                    // var url = actionContext.HttpContext.Request.Host + actionContext.HttpContext.Request.Path;
                    // var Method = actionContext.HttpContext.Request.Method.ToString();
                    // string actionName = actionContext.RouteData.Values["action"].ToString();
                    // string controllerName = actionContext.RouteData.Values["controller"].ToString();
                    //// await log.writeLogNewLine();
                    object logId = actionContext.HttpContext.Items["LogId"];
                    // //await log.writeLog("Log Id:" + logId);
                    // //await log.writeLog("Branch Id:" + _branchId);
                    // //await log.writeLog("User Id:" + _userId);
                    // //await log.writeLog("Url:" + url);
                    // //await log.writeLog("Controller Name:" + controllerName);
                    // //await log.writeLog("Action Name:" + actionName);
                    // //await log.writeLog("Method:" + Method);
                    var response = JsonConvert.SerializeObject(actionContext.Result);
                    //await log.writeLog("Response:" + response);
                    GlAuditLogUpdateRequest glAuditLogUpdateRequest = new GlAuditLogUpdateRequest();
                    glAuditLogUpdateRequest.LOG_ID = logId.ToString();
                    glAuditLogUpdateRequest.RESPONSE = response;
                    glAuditLogUpdateRequest.RESPONSE_TIME = DateTime.Now.ToLongTimeString();
                    await _logBase.updateAuditLog(glAuditLogUpdateRequest);
                }
            }
            catch (Exception ex)
            {

            }

        }

    }
}
