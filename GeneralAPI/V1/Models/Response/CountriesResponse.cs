using APIBaseClassLibrary.V1.Models.Response;
using  GeneralAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GlobalValues.GlobalVariables;

namespace GeneralAPI.V1.Models.Response
{
    public class CountriesResponse: BaseResponse
    {
        public List<CountriesProperties> countriesList { get; set; }

    }
}
