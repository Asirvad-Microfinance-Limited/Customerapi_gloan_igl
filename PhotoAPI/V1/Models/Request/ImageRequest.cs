using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAPI.V1.Models.Request
{
    public class ImageRequest
    {
        [Required(ErrorMessage ="Recording Id Required")]
        public string recordingId { get; set; }


        [Required(ErrorMessage = "Collection Name Required")]
        public string collectionName { get; set; }
        public string fileType { get; set; }

        public long editFlag { get; set; }
    }
}
