using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCAPI.V1.Models.Properties
{
    public class PawnBrokerFormProperties
    {
        [JsonProperty("formImage")]
        public string FORM_IMAGE { get; set; }
    }

    public class PawnBrokerLicenseProperties
    {
        [JsonProperty("licenseImage")]
        public string LICENSE_IMAGE { get; set; }
    }
}
