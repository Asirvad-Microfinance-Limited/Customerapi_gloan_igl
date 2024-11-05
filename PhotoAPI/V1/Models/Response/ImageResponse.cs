using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAPI.V1.Models.Response
{
    public class ImageData
    {
        public string imageString { get; set; }
        public string fileType { get; set; }
    }

    public class Data
    {
        public bool isDataAvilable { get; set; }
        public string imageString { get; set; }
        public string fileType { get; set; }
        public List<ImageData> imageData { get; set; }
    }

    public class ImageResponse
    {
        public string status { get; set; }
        public string apiStatus { get; set; }
        public string responseMsg { get; set; }
        public Data data { get; set; }
        public object errorList { get; set; }
    }
}
