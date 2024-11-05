using APIBaseClassLibrary.V1.Models.Request;
using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIBaseClassLibrary.V1.Log
{
    public interface ILogBase
    {
        Task<GlAuditLogResponse> addAuditLog(GlAuditLogRequest request);
        Task<GlAuditLogResponse> updateAuditLog(GlAuditLogUpdateRequest request);
        Task<GlErrorLogResponse> addErrorLog(GlErrorLogRequest request);
    }
}
