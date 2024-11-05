using APIBaseClassLibrary.V1.Log;
using APIBaseClassLibrary.V1.Models.Request;
using APIBaseClassLibrary.V1.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace APIBaseClassLibrary.V1.ExceptionHandler
{
    public class ExceptionMiddlewareExtensions 
    {
        private  readonly ILogBase _logBase;
        private readonly RequestDelegate _next;

        public ExceptionMiddlewareExtensions(RequestDelegate next, ILogBase logBase)
        {
            _logBase = logBase;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private   Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {

                var url = context.Items["url"].ToString();
                string Method = context.Items["Method"].ToString();
                string actionName = context.Items["actionName"].ToString();
                string controllerName = context.Items["controllerName"].ToString();
                string _userId = context.Items["_userId"].ToString();
                string _branchId = context.Items["_branchId"].ToString();
                object logId = context.Items["LogId"];

                string innerException = exception.InnerException != null ? exception.InnerException.ToString() : "null";

                GlErrorLogRequest glErrorLogRequest = new GlErrorLogRequest();
                glErrorLogRequest.LOG_ID = logId.ToString();
                glErrorLogRequest.BRANCH_ID = _branchId;
                glErrorLogRequest.EMP_CODE = _userId;
                glErrorLogRequest.URL = url;
                glErrorLogRequest.CONTROLLER_NAME = controllerName;
                glErrorLogRequest.ACTION_NAME = actionName;
                glErrorLogRequest.METHOD = Method;
                glErrorLogRequest.EXCEPTION = exception.Message;
                glErrorLogRequest.INNER_EXCEPTION = innerException.ToString();

                _logBase.addErrorLog(glErrorLogRequest);
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.logId = logId != null ? logId.ToString() : Guid.NewGuid().ToString();
                errorResponse.status.code = APIStatus.exception;
                errorResponse.status.message = exception.Message;
                errorResponse.status.flag = ProcessStatus.failed;
                HttpResponse response = context.Response;
                var responsedata = JsonConvert.SerializeObject(errorResponse);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(responsedata);
            }
            catch (Exception ex)
            {
                return null;
            }

        }




        //public async override void OnException(ExceptionContext actionContext)
        //{
        //    //ErrorLog log = new ErrorLog();
        //    ErrorResponse errorResponse = new ErrorResponse();
        //    string _branchId = "0";
        //    string _userId = "000000";
        //    StringValues authorizationToken;
        //    actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);

        //    if (!string.IsNullOrWhiteSpace(authorizationToken.ToString()))
        //    {
        //        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        //        var SecurityToken = handler.ReadToken(authorizationToken) as JwtSecurityToken;
        //        var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
        //        string[] value = jti.Split(',');
        //        _branchId = value[0];
        //        _userId = value[0];
        //    }
        //    else
        //    {
        //        _branchId = "0";
        //        _userId = "000000";
        //    }



        //    try
        //    {
        //        var url = actionContext.HttpContext.Request.Host + actionContext.HttpContext.Request.Path;
        //        var Method = actionContext.HttpContext.Request.Method.ToString();
        //        string actionName = actionContext.RouteData.Values["action"].ToString();
        //        string controllerName = actionContext.RouteData.Values["controller"].ToString();
        //        object logId = actionContext.HttpContext.Items["LogId"];
        //        //await log.writeLogNewLine();
        //        //await log.writeLog("Log Id:" + logId);
        //        //await log.writeLog("Branch Id:" + _branchId);
        //        //await log.writeLog("User Id:" + _userId);
        //        //await log.writeLog("Url:" + url);
        //        //await log.writeLog("Controller Name:" + controllerName);
        //        //await log.writeLog("Action Name:" + actionName);
        //        //await log.writeLog("Method:" + Method);
        //        //await log.writeLog("Exception:" + actionContext.Exception.Message);
        //        //await log.writeLogNewLine();

        //        var innerException = actionContext.Exception.InnerException != null ? actionContext.Exception.InnerException : null;
        //        var STACKTRACE = actionContext.Exception.StackTrace;
        //        GlErrorLogRequest glErrorLogRequest = new GlErrorLogRequest();
        //        glErrorLogRequest.LOG_ID = logId.ToString();
        //        glErrorLogRequest.BRANCH_ID = _branchId;
        //        glErrorLogRequest.EMP_CODE = _userId;
        //        glErrorLogRequest.URL = url;
        //        glErrorLogRequest.CONTROLLER_NAME = controllerName;
        //        glErrorLogRequest.ACTION_NAME = actionName;
        //        glErrorLogRequest.METHOD = Method;
        //        glErrorLogRequest.EXCEPTION = actionContext.Exception.Message;
        //        glErrorLogRequest.INNER_EXCEPTION = innerException.ToString();
        //        glErrorLogRequest.STACKTRACE = STACKTRACE;
        //        await _logBase.addErrorLog(glErrorLogRequest);

        //        errorResponse.logId = logId != null ? logId.ToString() : Guid.NewGuid().ToString();
        // //       errorResponse.fileName = "ErrorLog_" + DateTime.Now.ToString("yyyyMMddHH");
        //        errorResponse.status.code = APIStatus.exception;
        //        errorResponse.status.message = actionContext.Exception.Message;
        //        errorResponse.status.flag = ProcessStatus.failed;
        //        var exceptionType = actionContext.Exception.GetType();
        //        actionContext.ExceptionHandled = true;
        //        HttpResponse response = actionContext.HttpContext.Response;
        //        response.StatusCode = 500;
        //        response.ContentType = "application/json";
        //        var responsedata = JsonConvert.SerializeObject(errorResponse);



        //        await response.WriteAsync(responsedata);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
    }
}
