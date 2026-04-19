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
using static GlobalLibrary.GlobalVariables;

namespace APIBaseClassLibrary.V1.ExceptionHandler
{
    public class ExceptionMiddlewareExtensions : ExceptionFilterAttribute
    {
        public async override void OnException(ExceptionContext actionContext)
        {
            try
            {
                LogBase _logBase = new LogBase();
                ErrorResponse errorResponse = new ErrorResponse();
                
 
                var url = actionContext.HttpContext.Items["url"].ToString();
                string Method = actionContext.HttpContext.Items["Method"].ToString();
                string actionName = actionContext.HttpContext.Items["actionName"].ToString();
                string controllerName = actionContext.HttpContext.Items["controllerName"].ToString();
                string _userId = actionContext.HttpContext.Items["_userId"].ToString();
                string _branchId = actionContext.HttpContext.Items["_branchId"].ToString();
                object logId = actionContext.HttpContext.Items["LogId"];


                string innerException = actionContext.Exception.InnerException != null ? actionContext.Exception.InnerException.ToString() : "null";
                GlErrorLogRequest glErrorLogRequest = new GlErrorLogRequest();
                glErrorLogRequest.LOG_ID = logId.ToString();
                glErrorLogRequest.BRANCH_ID = _branchId;
                glErrorLogRequest.EMP_CODE = _userId;
                glErrorLogRequest.URL = url;
                glErrorLogRequest.CONTROLLER_NAME = controllerName;
                glErrorLogRequest.ACTION_NAME = actionName;
                glErrorLogRequest.METHOD = Method;
                glErrorLogRequest.EXCEPTION = actionContext.Exception.Message;
                glErrorLogRequest.INNER_EXCEPTION = innerException.ToString();
                await _logBase.addErrorLog(glErrorLogRequest);


                errorResponse.logId = logId != null ? logId.ToString() : Guid.NewGuid().ToString();
                errorResponse.status.code = APIStatus.exception;
                errorResponse.status.message = actionContext.Exception.Message;
                errorResponse.status.flag = ProcessStatus.failed;
                var exceptionType = actionContext.Exception.GetType();
                actionContext.ExceptionHandled = true;
                HttpResponse response = actionContext.HttpContext.Response;
                response.StatusCode = 500;
                response.ContentType = "application/json";
                var responsedata = JsonConvert.SerializeObject(errorResponse);
                await response.WriteAsync(responsedata);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
