using PANAPI.V1.Models.Request;
using PANAPI.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PANAPI.V1.BLLDependency
{
    public interface IPanBLL
    {
        PanDetailsResponse getPanDetails(PanDetailsRequest panDetailsRequest);
        UpdatePanResponse updatePan(UpdatePanDetailsRequest updatePanDetailsRequest);
        UpdateForm60Response updateForm60(UpdateForm60Request updateForm60Request);
        Form60DetailsResponse getForm60Details(Form60DeatilsRequest form60DeatilsRequest);
        SaveForm60DetailsResponse SaveForm60Details(SaveForm60DetailsRequest saveForm60Request);
    }
}
