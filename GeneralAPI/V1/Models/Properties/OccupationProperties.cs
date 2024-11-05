using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralAPI.V1.Models.Properties
{
    public class OccupationProperties

    {
        [JsonProperty("subCategoryId")]
        public long sub_category_id { get; set; }
        [JsonProperty("subCategoryName")]
        public string sub_cat_name { get; set; }
    }

    public class OccupationCategoryProperties

    {
        [JsonProperty("CategoryId")]
        public long category_id { get; set; }
        [JsonProperty("CategoryName")]
        public string category_name { get; set; }

    }

}
