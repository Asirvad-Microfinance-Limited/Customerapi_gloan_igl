using  GeneralAPI.V1.Models.Properties;
using APIBaseClassLibrary.V1.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Response
{
    public class OccupationSubCategoryResponse : BaseResponse
    {
        public List<OccupationCategoryProperties> occupationCategoryList { get; set; }

        public List<OccupationProperties> occupationSubCategoryList { get; set; }
    }
}
