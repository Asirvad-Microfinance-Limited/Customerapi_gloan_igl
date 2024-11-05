using APIBaseClassLibrary.V1.Models.Response;
using Newtonsoft.Json;
using PANAPI.V1.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PANAPI.V1.Models.Response
{
    public class PanCountResponse : BaseResponse
    {
        #region Declarations
        public long panCount { get; set; }

        #endregion Declarations

    }

    public class PanDetailsResponse : BaseResponse
    {
        #region Declarations

        public List<PanProperties> panList { get; set; }

        #endregion Declarations

    }


    public class UpdatePanResponse : BaseResponse
    {
        #region Declarations

        [JsonProperty("message")]
        public string message { get; set; }

        #endregion Declarations

    }

    public class UpdateForm60Response : BaseResponse
    {
      
        [JsonProperty("message")]
        public string message { get; set; }

     
    }

    public class Form60DetailsResponse : BaseResponse
    {
        #region Declarations

        public string transactionAmount { get; set; }
        public string transactionMode { get; set; }
        public string panApplyDate { get; set; }
        public string panAcknowledgeNo { get; set; }
        public string agriIncome { get; set; }
        public string otherAgriIncome { get; set; }
        public string documentCode { get; set; }
        public string documentIdNumber { get; set; }
        public string issuedBy { get; set; }
        public string form60Image { get; set; }

        #endregion Declarations

    }

    public class SaveForm60DetailsResponse : BaseResponse
    {

        [JsonProperty("message")]
        public string message { get; set; }


    }

     
}
