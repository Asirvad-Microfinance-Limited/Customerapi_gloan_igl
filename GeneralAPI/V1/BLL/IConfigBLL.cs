using GeneralAPI.V1.Models.Request;
using GeneralAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.BLL
{
    public interface IConfigBLL
    {
        ConfigInsertResponse insertConfig(ConfigInsertRequest request);
        ConfigUpdateResponse updateConfig(ConfigUpdateRequest request);
        ConfigDetailsResponse getConfig();

    }
}
